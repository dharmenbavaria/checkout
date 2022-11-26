using Checkout.Custom.Mock.Bank.Service;
using Checkout.Custom.Mock.Bank.Service.Model;
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
    public class ExecutePaymentHandler : IRequestHandler<ExecutePayment, Guid>
    {
        private readonly ICardsService _cardsService;
        private readonly ITransactionAuthCodeService _transactionAuthCodeService;
        private readonly IMediator _mediator;
        private readonly ILogger<ExecutePaymentHandler> _logger;

        public ExecutePaymentHandler(
            ICardsService cardsService,
            ITransactionAuthCodeService transactionAuthCodeService,
            IMediator mediator,
            ILogger<ExecutePaymentHandler> logger)
        {
            _cardsService = cardsService;
            _transactionAuthCodeService = transactionAuthCodeService;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Guid> Handle(ExecutePayment command, CancellationToken cancellationToken)
        {
            var transactionId = Guid.NewGuid();

            try
            {
                if (!_cardsService.Validate(command.CardDetails))
                {
                    var statusCode = HttpStatusCode.NotAcceptable.ToString();
                    var description = "Invalid card";

                    await _mediator.Publish(new PaymentExecuted(
                        transactionId: transactionId,
                        merchantId: command.MerchantId,
                        amount: command.Amount,
                        cardHolderName: command.CardDetails.CardHolderName,
                        encryptedCardNumber: _cardsService.Encrypt(command.CardDetails.CardNumber),
                        statusCode: statusCode,
                        description: description,
                        successful: false,
                        currency: command.Currency
                    ));

                    return transactionId;
                }

                var transactionAuthResponse = await _transactionAuthCodeService.VerifyAsync(new TransactionAuthRequest(
                        cardDetails: command.CardDetails,
                        amount: command.Amount));

                transactionId = transactionAuthResponse.TransactionId;
                await _mediator.Publish(new PaymentExecuted(
                         transactionId: transactionAuthResponse.TransactionId,
                         merchantId: command.MerchantId,
                         amount: command.Amount,
                         cardHolderName: command.CardDetails.CardHolderName,
                         encryptedCardNumber: _cardsService.Encrypt(command.CardDetails.CardNumber),
                         statusCode: transactionAuthResponse.Code,
                         description: transactionAuthResponse.Description,
                         successful: transactionAuthResponse.Verified,
                          currency: command.Currency
                     ));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled Exception for Command {Name} {@Command}",
                    nameof(ExecutePayment),
                    command);

                var statusCode = HttpStatusCode.ServiceUnavailable.ToString();
                var description = "Something went wrong. Please try again later.";
                await _mediator.Publish(new PaymentExecuted(
                        transactionId: transactionId,
                        merchantId: command.MerchantId,
                        amount: command.Amount,
                        cardHolderName: command.CardDetails.CardHolderName,
                        encryptedCardNumber: _cardsService.Encrypt(command.CardDetails.CardNumber),
                        statusCode: statusCode,
                        description: description,
                        successful: false,
                        currency: command.Currency
                    ));
            }

            return transactionId;
        }
    }
}
