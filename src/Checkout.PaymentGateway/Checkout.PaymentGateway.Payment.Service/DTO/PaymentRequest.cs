using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Application.DTO
{
    public class PaymentRequest
    {
        public int Amount { get; set; }

        public string CurrencyCode { get; set; }

        public string MerchantAccount { get; set; }

        public string MerchantReference { get; set; }

        public string CardNumber { get; set; }

        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }

        public string CVV { get; set; }

        public string HolderName { get; set; }

        public string PaymentMethod { get; set; }
    }
}
