using Checkout.PaymentGateway.Payment.Api.Models;
using Checkout.PaymentGateway.Payment.Application.DTO;
using Checkout.PaymentGateway.Payment.Application.Exceptions;
using Checkout.PaymentGateway.Payment.Application.Services;
using Checkout.PaymentGateway.Payment.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, IMediator mediator, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _mediator = mediator;
            _logger = logger;
        }
        /// <summary>
        /// Returns a payment containing all information that was sent in to create it
        /// </summary>
        /// <param name="id">The ID of a previously successfully created payment</param>
        /// <returns>A payment object</returns>
        /// <response code="200">Returns the requested payment</response>
        /// <respones code="404">Unable to find the requested payment</respones>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetPaymentResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation($"Received request to get paymeny with ID: {id}");

            var result = await _mediator.Send(new GetPaymentByIdQuery()
            {
                Id = id
            });

            if (result == null)
                return NotFound($"Unable to find payment with id: {id}");

            var paymentResult = new GetPaymentResponse()
            {
                Id = result.Id,
                Amount = result.Amount,
                CurrencyCode = result.CurrencyCode,
                MerchantAccount = result.MerchantDetails.MerchantAccount,
                MerchantReference = result.MerchantDetails.MerchantReference,
                Status = result.AcquirerResult.Status,
                CardNumber = result.PaymentDetails.CardNumber,
                ExpiryMonth = result.PaymentDetails.ExpiryMonth,
                ExpiryYear = result.PaymentDetails.ExpiryYear,
                CVV = result.PaymentDetails.CVV,
                HolderName = result.PaymentDetails.HolderName,
                PaymentMethod = result.PaymentDetails.PaymentMethod
            };

            _logger.LogInformation($"Payment with id: {id} found, returning to user");

            return Ok(paymentResult);
        }

        /// <summary>
        /// Creates a payment object with the specified properties
        /// </summary>
        /// <param name="idempotencyKey">Header specifiying a unique value of your choice that is relevant to this request only, used as a mechanism to combat idempotency</param>
        /// <param name="createPaymentRequest">The payment you want to submit</param>
        /// <returns>A payment result containing the id of your payment and whether or not it was successful</returns>
        /// <response code="201">Your payment has successfully been created</response>
        /// <response code="400">Request object was invalid</response>
        /// <response code="409">A payment with the same idempotency key has been created</response>
        /// <response code="500">An internal server error has occurred and your payment has not been created</response>
        [HttpPost]
        [ProducesResponseType(typeof(CreatePaymentResponse), (int)HttpStatusCode.Created)]
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromHeader(Name = "IdempotencyKey")] string idempotencyKey,  CreatePaymentRequest createPaymentRequest)
        {
            var paymentRequest = new PaymentRequest()
            {
                Amount = createPaymentRequest.Amount,
                CurrencyCode = createPaymentRequest.CurrencyCode,
                MerchantAccount = createPaymentRequest.MerchantAccount,
                MerchantReference = createPaymentRequest.MerchantReference,
                CardNumber = createPaymentRequest.PaymentDetails.CardNumber,
                ExpiryMonth = createPaymentRequest.PaymentDetails.ExpiryMonth,
                ExpiryYear = createPaymentRequest.PaymentDetails.ExpiryYear,
                CVV = createPaymentRequest.PaymentDetails.CVV,
                HolderName = createPaymentRequest.PaymentDetails.HolderName,
                PaymentMethod = createPaymentRequest.PaymentDetails.PaymentMethod,
            };

            try
            {
                _logger.LogInformation($"Attempting to create payment with amount: {paymentRequest.Amount}");

                var paymentResponse = await _paymentService.CreatePayment(idempotencyKey, paymentRequest);

                var result = new CreatePaymentResponse()
                {
                    Id = paymentResponse.Id,
                    MerchantReference = paymentResponse.MerchantReference,
                    Reason = paymentResponse.Reason,
                    Status = paymentResponse.AcquirerStatus
                };

                return Ok(result);
            }
            catch(InvalidPaymentTypeException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            catch(ConflictException ex)
            {
                _logger.LogError($"Existing payment found: {ex.Message}");
                return Conflict(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error when processing payment: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            } 


        }

    }
}
