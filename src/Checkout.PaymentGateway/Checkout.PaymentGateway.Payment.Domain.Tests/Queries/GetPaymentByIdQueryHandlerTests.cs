using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using Checkout.PaymentGateway.Payment.Domain.Queries;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Threading;
using Xunit;

namespace Checkout.PaymentGateway.Payment.Domain.Tests
{
    public class GetPaymentByIdQueryHandlerTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepository;
        private readonly Mock<ILogger<GetPaymentByIdQueryHandler>> _logger;
        private readonly GetPaymentByIdQueryHandler _handler;

        public GetPaymentByIdQueryHandlerTests()
        {
            _paymentRepository = new Mock<IPaymentRepository>();
            _logger = new Mock<ILogger<GetPaymentByIdQueryHandler>>();
            _handler = new GetPaymentByIdQueryHandler(_paymentRepository.Object, _logger.Object);
        }

        [Fact]
        public async void CallingHandle_ShouldCallRepository_ReturningNull()
        {
            var result = await _handler.Handle(new GetPaymentByIdQuery() { Id = 4 }, It.IsAny<CancellationToken>());

            _paymentRepository.Verify(m => m.GetPaymentByIdAsync(It.IsAny<int>()), Times.Once());
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

            _paymentRepository.Setup(e => e.GetPaymentByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(paymentObject);

            var result = await _handler.Handle(new GetPaymentByIdQuery() { Id = 1}, It.IsAny<CancellationToken>());

            _paymentRepository.Verify(m => m.GetPaymentByIdAsync(1), Times.Once());
            result.ShouldNotBe(null);
            result.Id.ShouldBe(1);
        }
    }
}
