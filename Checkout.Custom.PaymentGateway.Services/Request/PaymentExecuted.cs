using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services.Request
{
    public class PaymentExecuted : INotification
    {
        public PaymentExecuted(
            Guid transactionId,
            Guid merchantId,
            decimal amount,
            string cardHolderName,
            string encryptedCardNumber,
            string statusCode,
            string description,
            bool successful,
            string currency)
        {
            TransactionId = transactionId;
            MerchantId = merchantId;
            Amount = amount;
            CardHolderName = cardHolderName;
            EncryptedCardNumber = encryptedCardNumber;
            StatusCode = statusCode;
            Description = description;
            Successful = successful;
            Currency = currency;
        }

        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; }
        public string EncryptedCardNumber { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }

        public string Currency { get; set; }
        public bool Successful { get; set; }
    }
}
