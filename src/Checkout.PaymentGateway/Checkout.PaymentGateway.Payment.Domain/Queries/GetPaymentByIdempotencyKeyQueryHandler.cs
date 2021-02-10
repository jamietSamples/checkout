using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Domain.Queries
{
    public class GetPaymentByIdempotencyKeyQueryHandler : IRequestHandler<GetPaymentByIdompotencyKeyQuery, AggregatesModel.PaymentAggregate.Payment>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<GetPaymentByIdempotencyKeyQueryHandler> _logger;

        public GetPaymentByIdempotencyKeyQueryHandler(IPaymentRepository paymentRepository, ILogger<GetPaymentByIdempotencyKeyQueryHandler> logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public Task<AggregatesModel.PaymentAggregate.Payment> Handle(GetPaymentByIdompotencyKeyQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Querying payment repository for idempotency key: {request.IdempotencyKey}");

            return _paymentRepository.GetPaymentByIdempotencyKeyAsync(request.IdempotencyKey);
        }
    }
}
