using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using Checkout.PaymentGateway.Payment.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Linq;
using Xunit;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Tests
{
    public class PaymentRepositoryTests : DatabaseTestBase
    {
        private readonly ILogger<PaymentRepository> _logger;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentRepositoryTests()
        {
            _logger = new Mock<ILogger<PaymentRepository>>().Object;
            _paymentRepository = new PaymentRepository(PaymentContext, _logger);
        }

        [Fact]
        public void CallingGetPaymentById_WhenEntityDoesntExist_ReturnsNull()
        {
            var result = _paymentRepository.GetPaymentByIdAsync(4).Result;

            result.ShouldBe(null);
        }

        [Fact]
        public void CallingGetPaymentById_WhenEntityExists_ReturnsMatchingEntity()
        {
            var examplePayment = new Domain.AggregatesModel.PaymentAggregate.Payment()
            {
                Id = 1,
                Amount = 1000,
            };

            PaymentContext.Payments.Add(examplePayment);
            PaymentContext.SaveChanges();

            var result = _paymentRepository.GetPaymentByIdAsync(1).Result;

            result.Id.ShouldBe(1);
        }

        [Fact]
        public void CallingGetPaymentByIdempotencyKey_WhenEntityDoesntExist_ReturnsNull()
        {
            var result = _paymentRepository.GetPaymentByIdempotencyKeyAsync("specialKey").Result;

            result.ShouldBe(null);
        }

        [Fact]
        public void CallingGetPaymentByIdempotencyKey_WhenEntityExists_ReturnsMatchingEntity()
        {
            var examplePayment = new Domain.AggregatesModel.PaymentAggregate.Payment()
            {
                Id = 1,
                IdempotencyKey = "specialKey",
                Amount = 1000
            };

            PaymentContext.Payments.Add(examplePayment);
            PaymentContext.SaveChanges();

            var result = _paymentRepository.GetPaymentByIdempotencyKeyAsync("specialKey").Result;

            result.Id.ShouldBe(1);
            result.IdempotencyKey.ShouldBe("specialKey");
        }

        [Fact]
        public async void CallingCreatePayment_ShouldAddPayment()
        {
            var examplePayment = new Domain.AggregatesModel.PaymentAggregate.Payment()
            {
                Id = 1,
                IdempotencyKey = "specialKey",
                Amount = 1000
            };

            await _paymentRepository.CreatePaymentAsync(examplePayment);
            await _paymentRepository.SaveAsync();

            PaymentContext.Payments.Count().ShouldBe(1);
        }

    }
}
