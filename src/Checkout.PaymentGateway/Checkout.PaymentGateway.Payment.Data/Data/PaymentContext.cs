using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using Checkout.PaymentGateway.Payment.Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options)
            :base(options)
        {

        }

        public DbSet<Domain.AggregatesModel.PaymentAggregate.Payment> Payments { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PaymentConfiguration());
            builder.ApplyConfiguration(new PaymentMethodConfiguration());
        }
    }
}
