using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Application.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException()
        {
        }

        public ConflictException(string message)
            : base(message)
        {
        }

        public ConflictException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
