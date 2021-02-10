using Checkout.PaymentGateway.Acquirer.Models;
using Checkout.PaymentGateway.Acquirer.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Acquirer.Services
{
    public class AcquirerClientService : IAcquirerClientService
    {
        private readonly ILogger<AcquirerClientService> _logger;
        public AcquirerClientService(ILogger<AcquirerClientService> logger)
        {
            _logger = logger;
        }

        public async Task<AcquirerResponse> ProcessPayment(AcquirerRequest acquirerRequest)
        {
            var acquirerRef = Guid.NewGuid().ToString();

            var validator = new AcquirerRequestValidator();

            var result = await validator.ValidateAsync(acquirerRequest);

            _logger.LogInformation($"Payment validated. Result: {result.IsValid}");
            
            return new AcquirerResponse()
            {
                Status = result.IsValid ? "PROCESSED" : "REFUSED",
                AcquirerReference = acquirerRef,
                Reason = result.IsValid ? null : result.Errors.First().ErrorMessage
            };
        }
    }
}
