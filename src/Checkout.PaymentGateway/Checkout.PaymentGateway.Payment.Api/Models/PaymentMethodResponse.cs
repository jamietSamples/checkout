using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Api.Models
{
    public class PaymentMethodResponse
    {
        public string Type { get; set; }

        public string CurrencyCode { get; set; }

        public int MinAmount { get; set; }

        public int MaxAmount { get; set; }
    }
}
