using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using Checkout.PaymentGateway.Payment.Infrastructure.Data;
using System.Linq;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Tests
{
    public class DatabaseTestBasePopulater
    {
        public static void TryPopulate(PaymentContext paymentContext)
        {
            if (paymentContext.PaymentMethods.Any())
                return;

            Populate(paymentContext);
        }

        private static void Populate(PaymentContext paymentContext)
        {
            var paymentMethods = new[]
            {
                new PaymentMethod
                {
                    CurrencyCode = "GBP",
                    Type = "VISA",
                    MinAmount = 100,
                    MaxAmount = 10000
                },
                new PaymentMethod
                {
                    CurrencyCode = "GBP",
                    Type = "MASTERCARD",
                    MinAmount = 10001,
                    MaxAmount = 500000
                },
                new PaymentMethod
                {
                    CurrencyCode = "USD",
                    Type = "AMEX",
                    MinAmount = 100000,
                    MaxAmount = 10000000
                }
            };

            paymentContext.PaymentMethods.AddRange(paymentMethods);

            paymentContext.SaveChanges();
        }
    }
}
