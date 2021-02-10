using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Data
{
    public abstract class Repository
    {   
        private readonly PaymentContext _paymentContext;
        private readonly ILogger _logger;
        public Repository(PaymentContext paymentContext, ILogger logger)
        {
            _paymentContext = paymentContext;
            _logger = logger;
        }

        public async Task<bool> SaveAsync()
        {
            _logger.LogInformation($"Saving changes to payment context");

            return (await _paymentContext.SaveChangesAsync()) > 0;

        }
    }
}
