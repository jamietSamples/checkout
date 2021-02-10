using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate
{
    public class PaymentDetails
    {
        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public string CVV { get; set; }

        public string HolderName { get; set; }

        public string PaymentMethod { get; set; }
    }
}
