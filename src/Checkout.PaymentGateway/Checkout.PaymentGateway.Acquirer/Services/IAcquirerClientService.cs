using Checkout.PaymentGateway.Acquirer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Acquirer.Services
{
    public interface IAcquirerClientService
    {
        Task<AcquirerResponse> ProcessPayment(AcquirerRequest acquirerRequest);
    }
}
