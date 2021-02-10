using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Api.Models
{
    public class GetPaymentResponse
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public string CurrencyCode { get; set; }

        public string MerchantAccount { get; set; }

        public string MerchantReference { get; set; }

        public string Status { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public string CVV { get; set; }

        public string HolderName { get; set; }

        public string PaymentMethod { get; set; }
    }
}
