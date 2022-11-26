using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.Common.Model
{
    public class PaymentTransactionResponseVm
    {
        public PaymentTransactionResponseVm(
            Guid transactionId,
            Guid merchantId,
            string cardHolderName,
            string cardNumber,
            decimal amount,
            string statusCode,
            string description,
            DateTime timestamp,
            string currency)
        {
            TransactionId = transactionId;
            MerchantId = merchantId;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            Amount = amount;
            StatusCode = statusCode;
            Description = description;
            Timestamp = timestamp;
            Currency = currency;
        }

        public Guid TransactionId { get; set; }
        public Guid MerchantId { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string Currency { get;  set; }
        public bool Successful => string.IsNullOrEmpty(Description);
    }
}
