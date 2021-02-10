using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Domain.Commands
{
    public class CreatePaymentCommand : IRequest
    {
        public AggregatesModel.PaymentAggregate.Payment Payment { get; set; }
    }
}
