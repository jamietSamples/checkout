using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentByIdAsync(int Id);

        Task<Payment> GetPaymentByIdempotencyKeyAsync(string key);

        Task CreatePaymentAsync(Payment payment);

        Task<bool> SaveAsync();
    }
}
