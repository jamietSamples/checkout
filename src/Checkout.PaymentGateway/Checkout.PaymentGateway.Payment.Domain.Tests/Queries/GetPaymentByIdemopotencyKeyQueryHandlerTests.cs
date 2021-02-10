using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using Checkout.PaymentGateway.Payment.Domain.Queries;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Threading;
using Xunit;

namespace Checkout.PaymentGateway.Payment.Domain.Tests
{
    public class GetPaymentByIdemopotencyKeyQueryHandlerTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepository;
        private readonly Mock<ILogger<GetPaymentByIdempotencyKeyQueryHandler>> _logger;
        private readonly GetPaymentByIdempotencyKeyQueryHandler _handler;

        public GetPaymentByIdemopotencyKeyQueryHandlerTests()
        {
            _paymentRepository = new Mock<IPaymentRepository>();
            _logger = new Mock<ILogger<GetPaymentByIdempotencyKeyQueryHandler>>();
            _handler = new GetPaymentByIdempotencyKeyQueryHandler(_paymentRepository.Object, _logger.Object);
        }

        [Fact]
        public async void CallingHandle_ShouldCallRepository_ReturningNull()
        {
            var result = await _handler.Handle(new GetPaymentByIdompotencyKeyQuery() { IdempotencyKey = "yes" }, It.IsAny<CancellationToken>());

            _paymentRepository.Verify(m => m.GetPaymentByIdempotencyKeyAsync("yes"), Times.Once());
            result.ShouldBe(null);
        }


        [Fact]
        public async void CallingHandle_ShouldCallRepository_ReturningEntity()
        {
            var paymentObject = new AggregatesModel.PaymentAggregate.Payment()
            {
                Id = 1,
                Amount = 1000
            };

            _paymentRepository.Setup(e => e.GetPaymentByIdempotencyKeyAsync(It.IsAny<string>()))
                .ReturnsAsync(paymentObject);

            var result = await _handler.Handle(new GetPaymentByIdompotencyKeyQuery() { IdempotencyKey = "yes" }, It.IsAny<CancellationToken>());

            _paymentRepository.Verify(m => m.GetPaymentByIdempotencyKeyAsync("yes"), Times.Once());
            result.ShouldNotBe(null);
            result.Id.ShouldBe(1);
        }
    }
}
