using Checkout.Custom.PaymentGateway.Dto;
using Checkout.Custom.PaymentGateway.Services.Request;
using Checkout.Custom.PaymentGateway.Services.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services.Events
{
    public class PaymentExecutedHandler : INotificationHandler<PaymentExecuted>
    {
        private readonly IPaymentTransactionsService _paymentTransactionsService;
        private readonly ICardsService _cardsService;
        private readonly ILogger<PaymentExecutedHandler> _logger;

        public PaymentExecutedHandler(
            IPaymentTransactionsService paymentTransactionsService,
            ICardsService cardsService,
            ILogger<PaymentExecutedHandler> logger)
        {
            _paymentTransactionsService = paymentTransactionsService;
            _cardsService = cardsService;
            this._logger = logger;
        }

        public async Task Handle(PaymentExecuted @event, CancellationToken cancellationToken)
        {
            try
            {
                await _paymentTransactionsService.AddAsync(
                    new PaymentTransactionDto(
                        @event.TransactionId,
                        @event.MerchantId,
                        @event.Amount,
                        @event.CardHolderName,
                        @event.EncryptedCardNumber,
                        @event.StatusCode,
                        @event.Description,
                        @event.Currency));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled Exception for Event {Name} {@Event}",
                    nameof(PaymentExecuted),
                    @event);

                throw;
            }
        }
    }
}
