using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Application.Exceptions
{
    public class AcquirerException : Exception
    {
        public AcquirerException()
        {
        }

        public AcquirerException(string message)
            : base(message)
        {
        }

        public AcquirerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
