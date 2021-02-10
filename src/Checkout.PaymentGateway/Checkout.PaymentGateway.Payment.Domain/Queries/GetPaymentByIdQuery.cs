using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Domain.Queries
{
    public class GetPaymentByIdQuery : IRequest<AggregatesModel.PaymentAggregate.Payment>
    {
        public int Id { get; set; }
    }
}
