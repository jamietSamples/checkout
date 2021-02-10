using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Domain.Queries
{
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, AggregatesModel.PaymentAggregate.Payment>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<GetPaymentByIdQueryHandler> _logger;

        public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository, ILogger<GetPaymentByIdQueryHandler> logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public Task<AggregatesModel.PaymentAggregate.Payment> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Querying repository for payment with id {request.Id}");

            return _paymentRepository.GetPaymentByIdAsync(request.Id);
        }
    }
}
