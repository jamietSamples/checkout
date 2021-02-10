using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Data
{
    public class PaymentContextSeed
    {
        public static async Task SeedAsync(PaymentContext paymentContext)
        {
            if (!paymentContext.PaymentMethods.Any())
            {
                await paymentContext.PaymentMethods.AddRangeAsync(PopulateExistingPaymentMethods());

                await paymentContext.SaveChangesAsync();
            }
        }

        static IEnumerable<PaymentMethod> PopulateExistingPaymentMethods()
        {
            return new[]
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
        }
    }
}
