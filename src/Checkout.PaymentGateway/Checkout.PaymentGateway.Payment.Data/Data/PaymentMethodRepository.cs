using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Data
{
    public class PaymentMethodRepository : Repository, IPaymentMethodRepository
    {
        private readonly PaymentContext _paymentContext;
        private readonly ILogger<PaymentMethodRepository> _logger;

        public PaymentMethodRepository(PaymentContext paymentContext, ILogger<PaymentMethodRepository> logger)
            : base(paymentContext, logger)
        {
            _paymentContext = paymentContext;
            _logger = logger;
        }

        public Task<PaymentMethod[]> GetPaymentMethods()
        {
            _logger.LogInformation("Getting all payment methods");

            return _paymentContext.Set<PaymentMethod>().ToArrayAsync();
        }

        public Task<PaymentMethod[]> GetPaymentMethods(string currencyCode, int amount)
        {
            _logger.LogInformation($"Getting all available payment methods for amount: {amount} and currency: {currencyCode}");

            var query = _paymentContext.PaymentMethods
                .Where(pm => amount >= pm.MinAmount && amount <= pm.MaxAmount && pm.CurrencyCode == currencyCode);

            return query.ToArrayAsync();
        }
    }
}
