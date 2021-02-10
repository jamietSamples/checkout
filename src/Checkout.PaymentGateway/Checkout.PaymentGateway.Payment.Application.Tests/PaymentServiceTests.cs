using Checkout.PaymentGateway.Acquirer.Models;
using Checkout.PaymentGateway.Acquirer.Services;
using Checkout.PaymentGateway.Payment.Application.DTO;
using Checkout.PaymentGateway.Payment.Application.Exceptions;
using Checkout.PaymentGateway.Payment.Application.Services;
using Checkout.PaymentGateway.Payment.Domain.Commands;
using Checkout.PaymentGateway.Payment.Domain.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Threading;
using Xunit;

namespace Checkout.PaymentGateway.Payment.Application.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<ILogger<PaymentService>> _logger;
        private readonly Mock<IAcquirerClientService> _acquirerClientService;
        private readonly Mock<IMediator> _mediator;
        private readonly IPaymentService _paymentService;
        

        public PaymentServiceTests()
        {
            _logger = new Mock<ILogger<PaymentService>>();
            _acquirerClientService = new Mock<IAcquirerClientService>();
            _mediator = new Mock<IMediator>();
            _paymentService = new PaymentService(_acquirerClientService.Object, _mediator.Object, _logger.Object);
        }

        [Fact]
        public void CreatePayment_ShouldThrowConflictException_WhenIdempotencyKeyExists()
        {
            _mediator.Setup(e => e.Send(It.IsAny<GetPaymentByIdompotencyKeyQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Domain.AggregatesModel.PaymentAggregate.Payment() { Id = 1 });

            _acquirerClientService.Setup(e => e.ProcessPayment(It.IsAny<AcquirerRequest>()))
                .ReturnsAsync(It.IsAny<AcquirerResponse>());

            Should.Throw<ConflictException>(() => _paymentService.CreatePayment("sampleKey",It.IsAny<PaymentRequest>()));
        }

        [Fact]
        public void CreatePayment_ShouldThrowAcquirerException_WhenAcquirerRequestIsNull()
        {
            Should.Throw<AcquirerException>(() => _paymentService.CreatePayment(It.IsAny<string>(), It.IsAny<PaymentRequest>()));
        }


        [Fact]
        public void CreatePayment_ShouldCallCreatePaymentCommandOnce_WhenPaymentRequestIsValid()
        {
            var paymentRequest = new PaymentRequest()
            {
                Amount = 1100,
                CurrencyCode = "GBP",
                MerchantAccount = "merchant",
                MerchantReference = "aref",
                CardNumber = "5555 4444 3333 1111",
                ExpiryMonth = "08",
                ExpiryYear = "2021",
                CVV = "173",
                HolderName = "Mr string String",
                PaymentMethod = "VISA",
            };

            _acquirerClientService.Setup(e => e.ProcessPayment(It.IsAny<AcquirerRequest>()))
                .ReturnsAsync(new AcquirerResponse()
                {
                    AcquirerReference = "es",
                    Reason = null,
                    Status = "PROCESSED"
                });

            var result = _paymentService.CreatePayment(It.IsAny<string>(), paymentRequest).Result;

            _mediator.Verify(m => m.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        
    }
}
