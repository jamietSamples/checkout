using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Data.Configuration
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.PaymentAggregate.Payment>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.PaymentAggregate.Payment> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.OwnsOne(p => p.AcquirerResult);
            builder.OwnsOne(p => p.MerchantDetails);
            builder.OwnsOne(p => p.PaymentDetails);
        }
    }
}
