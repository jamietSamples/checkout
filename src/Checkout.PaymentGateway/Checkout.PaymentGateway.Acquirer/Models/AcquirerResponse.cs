using Checkout.PaymentGateway.Acquirer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Acquirer.Models
{
    public class AcquirerResponse
    {
        public string AcquirerReference { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }
    }
}
