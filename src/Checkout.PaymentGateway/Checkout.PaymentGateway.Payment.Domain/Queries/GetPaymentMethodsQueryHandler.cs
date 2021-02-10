using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Domain.Queries
{
    public class GetPaymentMethodsQueryHandler : IRequestHandler<GetPaymentMethodsQuery, PaymentMethod[]>
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly ILogger<GetPaymentMethodsQueryHandler> _logger;
        public GetPaymentMethodsQueryHandler(IPaymentMethodRepository paymentMethodRepository, ILogger<GetPaymentMethodsQueryHandler> logger)
        {
            _paymentMethodRepository = paymentMethodRepository;
            _logger = logger;
        }

        public Task<PaymentMethod[]> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Querying all available payment methods");

            return _paymentMethodRepository.GetPaymentMethods();
        }
    }
}
