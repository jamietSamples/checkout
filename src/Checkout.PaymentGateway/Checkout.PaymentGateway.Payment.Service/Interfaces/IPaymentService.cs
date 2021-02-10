using Checkout.PaymentGateway.Payment.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Application.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> CreatePayment(string idempotencyKey, PaymentRequest paymentRequest);
    }
}
