using Checkout.Custom.Common.Extensions;
using Checkout.Custom.Common.Model;
using Checkout.Custom.PaymentGateway.Services.Request;
using Checkout.Custom.PaymentGateway.Services.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Custom.PaymentGateway.Services.Command
{
    public class GetTransactionByMerchantIdQueryHandler : IRequestHandler<GetTransactionByMerchantId, List<PaymentTransactionResponseVm>>
    {
        private readonly ICardsService _cardsService;
        private readonly IPaymentTransactionsService _paymentTransactionsService;
        private readonly ILogger<GetTransactionByMerchantIdQueryHandler> _logger;

        public GetTransactionByMerchantIdQueryHandler(
            ICardsService cardsService,
            IPaymentTransactionsService paymentTransactionsService,
            ILogger<GetTransactionByMerchantIdQueryHandler> logger)
        {
            _cardsService = cardsService;
            _paymentTransactionsService = paymentTransactionsService;
            _logger = logger;
        }

        public async Task<List<PaymentTransactionResponseVm>> Handle(GetTransactionByMerchantId request, CancellationToken cancellationToken)
        {
            var response = new List<PaymentTransactionResponseVm>();
            try
            {
                var transactionItems = await _paymentTransactionsService.GetByMerchantIdAsync(request.MerchantId);
                return transactionItems?.Select(transactionItem =>
                    new PaymentTransactionResponseVm(
                        transactionId: transactionItem.Id,
                        merchantId: transactionItem.MerchantId,
                        cardHolderName: transactionItem.CardHolderName,
                        cardNumber: _cardsService.Decrypt(transactionItem.CardNumber).Mask('X'),
                        amount: transactionItem.Amount,
                        statusCode: transactionItem.StatusCode,
                        description: transactionItem.Description,
                        timestamp: transactionItem.Timestamp,
                        currency: transactionItem.Currency)).ToList() ?? response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Checkout Request: Unhandled Exception for Request {Name} {@Request}",
                    nameof(GetTransactionById),
                    request);
            }

            return response;
        }
    }
}
