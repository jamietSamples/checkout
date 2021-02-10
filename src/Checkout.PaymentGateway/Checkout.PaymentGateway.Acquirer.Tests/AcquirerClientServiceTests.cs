using Checkout.PaymentGateway.Acquirer.Models;
using Checkout.PaymentGateway.Acquirer.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace Checkout.PaymentGateway.Acquirer.Tests
{
    public class AcquirerClientServiceTests
    {
        private readonly IAcquirerClientService _acquirerClientService;

        public AcquirerClientServiceTests()
        {
            var logger = new Mock<ILogger<AcquirerClientService>>().Object;
            _acquirerClientService = new AcquirerClientService(logger);
        }

        [Fact]
        public async void ProcessPayment_ShouldReturnIsValid_WhenAcquirerRequestSubmitted()
        {
            var acquirerRequest = new AcquirerRequest()
            {
                Amount = 1000,
                CurrencyCode = "GBP",
                CardNumber = "5555 4444 3333 1111",
                ExpiryMonth = "08",
                ExpiryYear = "2021",
                CVV = "322",
                HolderName = "Jamie Jam",
                PaymentMethod = "VISA"
            };

            var result = await _acquirerClientService.ProcessPayment(acquirerRequest);

            result.Status.ShouldBe("PROCESSED");
            result.AcquirerReference.ShouldNotBe(null);
            result.Reason.ShouldBe(null);
        }

        [Fact]
        public async void ProcessPayment_ShouldReturnIsInvalid_WhenAcquirerRequestSubmitted()
        {
            var acquirerRequest = new AcquirerRequest()
            {
                Amount = 1000,
                CurrencyCode = "AED",
                CardNumber = "5555 4444 3333 1111",
                ExpiryMonth = "08",
                ExpiryYear = "2021",
                CVV = "322",
                HolderName = "Jamie Jam",
                PaymentMethod = "VISA"
            };

            var result = await _acquirerClientService.ProcessPayment(acquirerRequest);

            result.Status.ShouldBe("REFUSED");
            result.AcquirerReference.ShouldNotBe(null);
            result.Reason.ShouldBe("Invalid currency specified");
        }
    }
}
