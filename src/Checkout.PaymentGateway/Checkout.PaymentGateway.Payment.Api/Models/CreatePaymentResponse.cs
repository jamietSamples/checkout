using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Api.Models
{
    public class CreatePaymentResponse
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }

        public string MerchantReference { get; set; }
    }
}
