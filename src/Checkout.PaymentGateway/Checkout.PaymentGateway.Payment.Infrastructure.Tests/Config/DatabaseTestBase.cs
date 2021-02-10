using Checkout.PaymentGateway.Payment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Infrastructure.Tests
{
    public class DatabaseTestBase : IDisposable
    {
        protected readonly PaymentContext PaymentContext;

        public DatabaseTestBase()
        {
            var dbContextOptions = new DbContextOptionsBuilder<PaymentContext>().UseInMemoryDatabase("payments");

            PaymentContext = new PaymentContext(dbContextOptions.Options);

            PaymentContext.Database.EnsureCreated();

            DatabaseTestBasePopulater.TryPopulate(PaymentContext);
        }

        public void Dispose()
        {
            PaymentContext.Database.EnsureDeleted();

            PaymentContext.Dispose();
        }
    }
}
