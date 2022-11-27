using System.Net;
using Checkout.Custom.Mock.Bank.Data.Repository;
using Checkout.Custom.Mock.Bank.Service.Model;
using Microsoft.Extensions.Logging;

namespace Checkout.Custom.Mock.Bank.Service
{
    public interface ITransactionAuthCodeService
    {
        Task<TransactionAuthResponse> VerifyAsync(TransactionAuthRequest payload);
    }

    public class TransactionAuthCodeService : ITransactionAuthCodeService
    {
        private readonly ITransactionAuthCodeRepository _bankAuthProvider;
        private readonly ILogger<TransactionAuthCodeService> _logger;

        public TransactionAuthCodeService(
            ITransactionAuthCodeRepository bankAuthProvider,
            ILogger<TransactionAuthCodeService> logger)
        {
            _bankAuthProvider = bankAuthProvider;
            _logger = logger;
        }

        public async Task<TransactionAuthResponse> VerifyAsync(TransactionAuthRequest payload)
        {
            var transactionId = Guid.NewGuid();
            if (payload.Amount <= 0)
                return new TransactionAuthResponse(
                    transactionId: transactionId,
                    verified: false,
                    code: HttpStatusCode.NotAcceptable.ToString(),
                    description: "Amount not accepted"
                );

            try
            {
                var transactionAuth = await _bankAuthProvider.ValidateAsync(payload.Amount);
                return transactionAuth == null ?
                    new TransactionAuthResponse(
                        transactionId: transactionId,
                        verified: true,
                        code: "10000",
                        description: "Successful"
                    ) :
                    new TransactionAuthResponse(
                        transactionId: transactionId,
                        verified: false,
                        code: transactionAuth.TransactionCode,
                        description: transactionAuth.Description
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled Exception for Command {Name} {@Command}",
                    nameof(TransactionAuthCodeService),
                    payload);
            }

            return new TransactionAuthResponse(
                        transactionId: transactionId,
                        verified: true,
                        code: HttpStatusCode.ServiceUnavailable.ToString(),
                        description: "The verification could not be performed"
                    );
        }
    }
}