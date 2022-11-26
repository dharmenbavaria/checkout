using Checkout.Custom.Common.Extensions;
using Checkout.Custom.Common.Model;
using Checkout.Custom.PaymentGateway.Services.Request;
using Checkout.Custom.PaymentGateway.Services.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services.Command
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionById, PaymentTransactionResponseVm>
    {
        private readonly ICardsService _cardsService;
        private readonly IPaymentTransactionsService _paymentTransactionsService;
        private readonly ILogger<GetTransactionByIdQueryHandler> _logger;

        public GetTransactionByIdQueryHandler(
            ICardsService cardsService,
            IPaymentTransactionsService paymentTransactionsService,
            ILogger<GetTransactionByIdQueryHandler> logger)
        {
            _cardsService = cardsService;
            _paymentTransactionsService = paymentTransactionsService;
            _logger = logger;
        }

        public async Task<PaymentTransactionResponseVm> Handle(GetTransactionById request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _paymentTransactionsService.GetByTransactionIdAsync(request.Id);
                return result != null ?
                    new PaymentTransactionResponseVm(
                        transactionId: result.Id,
                        merchantId: result.MerchantId,
                        cardHolderName: result.CardHolderName,
                        cardNumber: _cardsService.Decrypt(result.CardNumber).Mask('X'),
                        amount: result.Amount,
                        statusCode: result.StatusCode,
                        description: result.Description,
                        timestamp: result.Timestamp,
                        currency: result.Currency) :
                    new PaymentTransactionResponseVm(
                        transactionId: request.Id,
                        merchantId: Guid.Empty,
                        cardHolderName: string.Empty,
                        cardNumber: string.Empty,
                        amount: 0,
                        statusCode: HttpStatusCode.NotFound.ToString(),
                        description: "The requested transaction could not be found",
                        timestamp: DateTime.MinValue,
                        currency: string.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Checkout Request: Unhandled Exception for Request {Name} {@Request}",
                    nameof(GetTransactionById),
                    request);
            }

            return new PaymentTransactionResponseVm(
                        transactionId: request.Id,
                        merchantId: Guid.Empty,
                        cardHolderName: string.Empty,
                        cardNumber: string.Empty,
                        amount: 0,
                        statusCode: HttpStatusCode.ServiceUnavailable.ToString(),
                        description: "Unfortunately It was not possible to process your request",
                        timestamp: DateTime.MinValue,
                        currency: string.Empty);
        }
    }
}
