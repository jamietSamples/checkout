using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using Checkout.PaymentGateway.Payment.Domain.Queries;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Checkout.PaymentGateway.Payment.Domain.Tests
{
    public class GetPaymentMethodsQueryHandlerTests
    {
        private readonly Mock<IPaymentMethodRepository> _paymentMethodRepository;
        private readonly Mock<ILogger<GetPaymentMethodsQueryHandler>> _logger;
        private readonly GetPaymentMethodsQueryHandler _handler;

        public GetPaymentMethodsQueryHandlerTests()
        {
            _paymentMethodRepository = new Mock<IPaymentMethodRepository>();
            _logger = new Mock<ILogger<GetPaymentMethodsQueryHandler>>();
            _handler = new GetPaymentMethodsQueryHandler(_paymentMethodRepository.Object, _logger.Object);
        }

        [Fact]
        public async void CallingHandle_ShouldCallRepository_ReturningNull()
        {
            var result = await _handler.Handle(new GetPaymentMethodsQuery(), It.IsAny<CancellationToken>());

            _paymentMethodRepository.Verify(m => m.GetPaymentMethods(), Times.Once());
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


            _paymentMethodRepository.Setup(e => e.GetPaymentMethods())
                .ReturnsAsync(paymentMethods);

            var result = await _handler.Handle(new GetPaymentMethodsQuery(), It.IsAny<CancellationToken>());

            _paymentMethodRepository.Verify(m => m.GetPaymentMethods(), Times.Once());
            result.ShouldNotBe(null);
            result.Length.ShouldBe(1);
        }
    }
}
