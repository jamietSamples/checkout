using Checkout.PaymentGateway.Payment.Domain.SeedWork;

namespace Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate
{
    public class Payment : IAggregateRoot
    {
        public int Id { get; set; }

        public string IdempotencyKey { get; set; }

        public int Amount { get; set; }

        public string CurrencyCode { get; set; }

        public MerchantDetails MerchantDetails { get; set; }

        public PaymentDetails PaymentDetails { get; set; }

        public AcquirerResult AcquirerResult { get; set; }

    }
}
