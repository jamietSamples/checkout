using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentMethodAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Domain.Queries
{
    public class GetPaymentMethodsByCurrencyQueryHandler : IRequestHandler<GetPaymentMethodsByCurrencyQuery, PaymentMethod[]>
    {

        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly ILogger<GetPaymentMethodsByCurrencyQueryHandler> _logger;
        public GetPaymentMethodsByCurrencyQueryHandler(IPaymentMethodRepository paymentMethodRepository, ILogger<GetPaymentMethodsByCurrencyQueryHandler> logger)
        {
            _paymentMethodRepository = paymentMethodRepository;
            _logger = logger;
        }

        public Task<PaymentMethod[]> Handle(GetPaymentMethodsByCurrencyQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Querying payment methods for amount: {request.Amount} and currency: {request.CurrencyCode}");

            return _paymentMethodRepository.GetPaymentMethods(request.CurrencyCode,request.Amount);
        }
    }
}
