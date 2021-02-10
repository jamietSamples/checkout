using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Data
{
    public class PaymentRepository : Repository, IPaymentRepository
    {
        private readonly PaymentContext _paymentContext;
        private readonly ILogger<PaymentRepository> _logger;

        public PaymentRepository(PaymentContext paymentContext, ILogger<PaymentRepository> logger)
            :base(paymentContext,logger)
        {
            _paymentContext = paymentContext;
            _logger = logger;
        }


        public Task<Domain.AggregatesModel.PaymentAggregate.Payment> GetPaymentByIdAsync(int id)
        {
            _logger.LogInformation($"Retrieving payment with id: {id}");

            return _paymentContext.Payments.FirstOrDefaultAsync(p => p.Id == id);

        }

        public Task<Domain.AggregatesModel.PaymentAggregate.Payment> GetPaymentByIdempotencyKeyAsync(string key)
        {
            _logger.LogInformation($"Retrieving payment with idempotency key: {key}");

            return _paymentContext.Payments.FirstOrDefaultAsync(p => p.IdempotencyKey == key);

        }

        public Task CreatePaymentAsync(Domain.AggregatesModel.PaymentAggregate.Payment payment)
        {
            _logger.LogInformation($"Creating payment with amount: {payment.Amount}");

            return _paymentContext.Payments.AddAsync(payment).AsTask();
        }
    }
}
