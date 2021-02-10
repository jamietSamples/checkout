using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using Checkout.PaymentGateway.Payment.Domain.Queries;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq;
using System.Threading;
using Xunit;

namespace Checkout.PaymentGateway.Payment.Domain.Tests
{
    public class GetPaymentMethodsByCurrencyQueryHandlerTests
    {
        private readonly Mock<IPaymentMethodRepository> _paymentMethodRepository;
        private readonly Mock<ILogger<GetPaymentMethodsByCurrencyQueryHandler>> _logger;
        private readonly GetPaymentMethodsByCurrencyQueryHandler _handler;

        public GetPaymentMethodsByCurrencyQueryHandlerTests()
        {
            _paymentMethodRepository = new Mock<IPaymentMethodRepository>();
            _logger = new Mock<ILogger<GetPaymentMethodsByCurrencyQueryHandler>>();
            _handler = new GetPaymentMethodsByCurrencyQueryHandler(_paymentMethodRepository.Object, _logger.Object);
        }

        [Fact]
        public async void CallingHandle_ShouldCallRepository_ReturningNull()
        {
            var result = await _handler.Handle(new GetPaymentMethodsByCurrencyQuery() { Amount = 10, CurrencyCode = "AED" }, It.IsAny<CancellationToken>());

            _paymentMethodRepository.Verify(m => m.GetPaymentMethods(It.IsAny<string>(),It.IsAny<int>()), Times.Once());
            result.ShouldBe(new PaymentMethod[] { });
        }

        [Fact]
        public async void CallingHandle_ShouldCallRepository_ReturningEntity()
        {
            var paymentMethods = new PaymentMethod[] {
                new PaymentMethod
                {
                    CurrencyCode = "GBP",
                    Type = "VISA",
                    MinAmount = 100,
                    MaxAmount = 10000
                }
            };

            var paymentMethodsAlternate = new PaymentMethod[] {
                new PaymentMethod
                {
                    CurrencyCode = "USD",
                    Type = "AMEX",
                    MinAmount = 100000,
                    MaxAmount = 10000000
                }
            };

            _paymentMethodRepository.Setup(e => e.GetPaymentMethods("GBP" ,It.IsAny<int>()))
                .ReturnsAsync(paymentMethods);

            _paymentMethodRepository.Setup(e => e.GetPaymentMethods("USD", It.IsAny<int>()))
                .ReturnsAsync(paymentMethodsAlternate);

            var result = await _handler.Handle(new GetPaymentMethodsByCurrencyQuery() { Amount = 300, CurrencyCode = "GBP" }, It.IsAny<CancellationToken>());

            _paymentMethodRepository.Verify(m => m.GetPaymentMethods(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
            result.ShouldNotBe(null);
            result.First().Type.ShouldBe("VISA");
        }
    }
}
