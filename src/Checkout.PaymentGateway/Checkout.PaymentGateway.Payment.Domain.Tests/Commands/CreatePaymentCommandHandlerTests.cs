using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using Checkout.PaymentGateway.Payment.Domain.Commands;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.PaymentGateway.Payment.Domain.Tests
{
    public class CreatePaymentCommandHandlerTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepository;
        private readonly Mock<ILogger<CreatePaymentCommandHandler>> _logger;
        private readonly CreatePaymentCommandHandler _handler;

        public CreatePaymentCommandHandlerTests()
        {
            _paymentRepository = new Mock<IPaymentRepository>();
            _logger = new Mock<ILogger<CreatePaymentCommandHandler>>();
            _handler = new CreatePaymentCommandHandler(_paymentRepository.Object, _logger.Object);
        }

        [Fact]
        public async void CallingHandle_ShouldCallRepository_CreatePaymentAsync()
        {
            var paymentObject = new AggregatesModel.PaymentAggregate.Payment()
            {
                Id = 1,
                Amount = 1000
            };

            _paymentRepository.Setup(e => e.CreatePaymentAsync(It.IsAny<AggregatesModel.PaymentAggregate.Payment>()))
                .Returns(Task.FromResult(paymentObject));

            await _handler.Handle(new CreatePaymentCommand() { Payment = paymentObject}, It.IsAny<CancellationToken>());

            _paymentRepository.Verify(m => m.CreatePaymentAsync(It.IsAny<AggregatesModel.PaymentAggregate.Payment>()), Times.Once());
            _paymentRepository.Verify(m => m.SaveAsync(), Times.Once());
        }
    }
}
