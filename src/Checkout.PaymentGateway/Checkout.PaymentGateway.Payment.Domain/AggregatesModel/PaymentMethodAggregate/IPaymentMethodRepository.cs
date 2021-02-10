using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate
{
    public interface IPaymentMethodRepository
    {
        Task<PaymentMethod[]> GetPaymentMethods();

        Task<PaymentMethod[]> GetPaymentMethods(string currencyCode, int amount);

        Task<bool> SaveAsync();
    }
}
