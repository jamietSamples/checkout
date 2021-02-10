using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.PaymentGateway.Payment.Application.Exceptions
{
    public class InvalidPaymentTypeException : Exception
    {
        public InvalidPaymentTypeException()
        {
        }

        public InvalidPaymentTypeException(string message)
            : base(message)
        {
        }

        public InvalidPaymentTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
