using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate
{
    public class MerchantDetails
    {
        public string MerchantAccount { get; set; }

        public string MerchantReference { get; set; }
    }
}
