using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Application.DTO
{
    public class PaymentResponse
    {
        public int Id { get; set; }

        public string AcquirerStatus { get; set; }

        public string Reason { get; set; }

        public string MerchantReference { get; set; }
    }
}
