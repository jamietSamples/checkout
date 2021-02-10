using Checkout.PaymentGateway.Payment.Domain.SeedWork;

namespace Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate
{
    public class PaymentMethod : BaseEntity, IAggregateRoot
    {
        //public int Id { get; set; }

        public string Type { get; set; }

        public string CurrencyCode { get; set; }

        public int MinAmount { get; set; }

        public int MaxAmount { get; set; }
    }
}
