using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate
{
    public class AcquirerResult
    {
        public string Reference { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }
    }
}
