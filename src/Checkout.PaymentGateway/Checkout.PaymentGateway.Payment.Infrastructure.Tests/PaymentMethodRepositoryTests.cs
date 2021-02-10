using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using Checkout.PaymentGateway.Payment.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq;
using Xunit;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Tests
{
    public class PaymentMethodRepositoryTests : DatabaseTestBase
    {
        private readonly ILogger<PaymentMethodRepository> _logger;
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodRepositoryTests()
        {
            _logger = new Mock<ILogger<PaymentMethodRepository>>().Object;
            _paymentMethodRepository = new PaymentMethodRepository(PaymentContext, _logger);
        }

        [Fact]
        public void GetPaymentMethods_ShouldReturnAllPaymentMethods()
        {
            var numberOfEntities = PaymentContext.PaymentMethods.Count();

            var result = _paymentMethodRepository.GetPaymentMethods();

            numberOfEntities.ShouldBeEquivalentTo(result.Result.Length);
        }

        [Theory]
        [InlineData("GBP", 1000, "VISA")]
        [InlineData("GBP", 10500, "MASTERCARD")]
        [InlineData("USD", 100500, "AMEX")]
        public void GetPaymentMethods_ShouldReturnSpecifiedMethod(string currencyCode, int amount, string expectedType)
        {
            var result = _paymentMethodRepository.GetPaymentMethods(currencyCode,amount).Result;

            result.Count().ShouldBe(1);
            result.First().Type.ShouldBe(expectedType);
        }

        [Theory]
        [InlineData("EUR",1000)]
        public void GetPaymentMethods_ShouldReturnEmptyArray(string currencyCode, int amount)
        {
            var result = _paymentMethodRepository.GetPaymentMethods(currencyCode, amount).Result;

            result.Count().ShouldBe(0);
        }


    }
}
