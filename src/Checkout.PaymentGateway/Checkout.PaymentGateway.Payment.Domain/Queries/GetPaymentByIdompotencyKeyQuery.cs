using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Domain.Queries
{
    public class GetPaymentByIdompotencyKeyQuery : IRequest<AggregatesModel.PaymentAggregate.Payment>
    {
        public string IdempotencyKey { get; set; }
    }
}
