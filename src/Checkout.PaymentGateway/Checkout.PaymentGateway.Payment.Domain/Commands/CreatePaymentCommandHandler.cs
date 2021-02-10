using Checkout.PaymentGateway.Payment.Domain.AggregatesModel.PaymentAggregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Domain.Commands
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger _logger;
        public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, ILogger<CreatePaymentCommandHandler> logger)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating payment");

            await _paymentRepository.CreatePaymentAsync(request.Payment);

            _logger.LogInformation($"Saving payment with id: {request.Payment.Id}");

            await _paymentRepository.SaveAsync();

            return Unit.Value;
        }
    }
}
