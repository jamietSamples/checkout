using Checkout.PaymentGateway.Acquirer.Models;
using Checkout.PaymentGateway.Acquirer.Services;
using Checkout.PaymentGateway.Payment.Application.DTO;
using Checkout.PaymentGateway.Payment.Application.Exceptions;
using Checkout.PaymentGateway.Payment.Application.Extensions;
using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using Checkout.PaymentGateway.Payment.Domain.Commands;
using Checkout.PaymentGateway.Payment.Domain.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAcquirerClientService _acquirerClientService;
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IAcquirerClientService acquirerClientService, IMediator mediator, ILogger<PaymentService> logger)
        {
            _mediator = mediator;
            _acquirerClientService = acquirerClientService;
            _logger = logger;
        }

        public async Task<PaymentResponse> CreatePayment(string idempotencyKey, PaymentRequest paymentRequest)
        {
            //await ValidatePaymentType(paymentRequest.Amount, paymentRequest.CurrencyCode, paymentRequest.PaymentMethod);

            await FindMatchingPaymentByIdempotencyKey(idempotencyKey);

            var acquirerResponse = await ProcessPaymentWithAcquirer(paymentRequest);

            var payment = new Domain.AggregatesModel.PaymentAggregate.Payment()
            {
                Amount = paymentRequest.Amount,
                CurrencyCode = paymentRequest.CurrencyCode,
                IdempotencyKey = idempotencyKey,
                PaymentDetails = new PaymentDetails()
                {
                    CardNumber = paymentRequest.CardNumber.Mask(paymentRequest.CardNumber.Length - 4),
                    CVV = paymentRequest.CVV,
                    ExpiryMonth = paymentRequest.ExpiryMonth,
                    ExpiryYear = paymentRequest.ExpiryYear,
                    HolderName = paymentRequest.HolderName,
                    PaymentMethod = paymentRequest.PaymentMethod
                },
                AcquirerResult = new AcquirerResult()
                {
                    Reason = acquirerResponse.Reason,
                    Status = acquirerResponse.Status,
                    Reference = acquirerResponse.AcquirerReference,
                },
                MerchantDetails = new MerchantDetails()
                {
                    MerchantAccount = paymentRequest.MerchantAccount,
                    MerchantReference = paymentRequest.MerchantReference
                }
            };

            await _mediator.Send(new CreatePaymentCommand()
            {
                Payment = payment
            });

            return new PaymentResponse()
            {
                Id = payment.Id,
                AcquirerStatus = acquirerResponse.Status,
                Reason = acquirerResponse.Reason,
                MerchantReference = paymentRequest.MerchantReference,
            };
        }

        private async Task ValidatePaymentType(int amount, string currencyCode, string type)
        {
            var allowedTypes = await _mediator.Send(new GetPaymentMethodsByCurrencyQuery()
            {
                Amount = amount,
                CurrencyCode = currencyCode,
            });

            var matchFound = allowedTypes.Any(pm => pm.Type == type.ToUpper());

            if (!matchFound)
            {
                throw new InvalidPaymentTypeException($"Invalid payment type specified");
            }
        }

        private async Task FindMatchingPaymentByIdempotencyKey(string idempotencyKey)
        {
            _logger.LogInformation($"Searching payments for idempotency key: {idempotencyKey}");

            if (string.IsNullOrEmpty(idempotencyKey))
                return;

            var matchingKeyResult = await _mediator.Send(new GetPaymentByIdompotencyKeyQuery()
            {
                IdempotencyKey = idempotencyKey
            });

            if (matchingKeyResult != null)
            {
                _logger.LogInformation($"Matching entity with same payment id found. Id: {matchingKeyResult.Id}");
                throw new ConflictException($"Matching entity with same payment id found. Id: {matchingKeyResult.Id}");
            }
        }

        private async Task<AcquirerResponse> ProcessPaymentWithAcquirer(PaymentRequest paymentRequest)
        {
            _logger.LogInformation("Attempting to process payment with acquirer");

            try
            {
                var acquirerRequest = new AcquirerRequest()
                {
                    Amount = paymentRequest.Amount,
                    CardNumber = paymentRequest.CardNumber,
                    CurrencyCode = paymentRequest.CurrencyCode,
                    CVV = paymentRequest.CVV,
                    ExpiryMonth = paymentRequest.ExpiryMonth,
                    ExpiryYear = paymentRequest.ExpiryYear,
                    PaymentMethod = paymentRequest.PaymentMethod,
                    HolderName = paymentRequest.HolderName
                };

                return await _acquirerClientService.ProcessPayment(acquirerRequest);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Failed to process payment: {ex.Message}");
                throw new AcquirerException($"Acquirer failed to process payment: {ex.Message}");
            }
        }
    }
}
