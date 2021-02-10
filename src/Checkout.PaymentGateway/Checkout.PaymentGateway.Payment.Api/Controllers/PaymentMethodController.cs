using Checkout.PaymentGateway.Payment.Api.Models;
using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using Checkout.PaymentGateway.Payment.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentMethodController> _logger;

        public PaymentMethodController(IMediator mediator, ILogger<PaymentMethodController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Query this endpoint in order to obtain a list of all viable payment methods, this can be cached client side in order to determine what amounts / currencies can be used with a type. Such as VISA, AMEX etc
        /// </summary>
        /// <returns>List of all available payment methods</returns>
        /// <response code="200">Payment Methods successfully retrieved</response>
        /// <response code="500">Failed to process the request</response>
        [HttpGet]
        [ProducesResponseType(typeof(PaymentMethodResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> GetAll()
        {
            var paymentMethods = await _mediator.Send(new GetPaymentMethodsQuery());

            var response = ConvertToPaymentMethodResponse(paymentMethods);

            _logger.LogInformation("Successfully retrieved all payment methods");

            return Ok(response);
        }

        /// <summary>
        /// Used to obtain a filtered list of payment methods, this would be called in advance of a transaction in order to verify what payment methods are available for that transaction (not required)
        /// </summary>
        /// <param name="currencyCode">Three letter currency code, e.g "GBP" "EUR"</param>
        /// <param name="amount">Amount in minor units, e.g 123 in GBP = £1.23</param>
        /// <returns>Filtered payment methods</returns>
        /// <response code="200">Payment Methods successfully retrieved</response>
        /// <response code="500">Failed to process the request</response>
        [HttpGet("{currencyCode}/{amount}")]
        [ProducesResponseType(typeof(PaymentMethodResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> Get(string currencyCode, int amount)
        {
            _logger.LogInformation($"Requesting all payment methods that satisfy the amount: {amount} and currency: {currencyCode}");

            var paymentMethods = await _mediator.Send(new GetPaymentMethodsByCurrencyQuery()
            {
                CurrencyCode = currencyCode,
                Amount = amount
            });

            var response = ConvertToPaymentMethodResponse(paymentMethods);

            _logger.LogInformation($"Return all payment methods that met criteria. Amount: {amount} Currency: {currencyCode}");

            return Ok(response);
        }

        private static IEnumerable<PaymentMethodResponse> ConvertToPaymentMethodResponse(PaymentMethod[] paymentMethods)
        {
            return paymentMethods.Select(pm => new PaymentMethodResponse
            {
                Type = pm.Type,
                CurrencyCode = pm.CurrencyCode,
                MaxAmount = pm.MaxAmount,
                MinAmount = pm.MinAmount

            });
        }
    }
}
