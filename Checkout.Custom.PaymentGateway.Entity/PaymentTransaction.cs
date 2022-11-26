using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.Custom.Common.Entity;

namespace Checkout.Custom.PaymentGateway.Entity
{
    public class PaymentTransaction : IEntity
    {
        public PaymentTransaction()
        {
        }

        public PaymentTransaction(
            Guid id,
            Guid merchantId,
            decimal amount,
            string cardHolderName,
            string cardNumber,
            string statusCode,
            string description,
            string currency)
        {
            Id = id;
            MerchantId = merchantId;
            Amount = amount;
            CardHolderName = cardHolderName;
            CardNumber = cardNumber;
            StatusCode = statusCode;
            Description = description;
            Timestamp = DateTime.Now;
            Currency = currency;
        }

        [Key]
        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public bool Successful => string.IsNullOrEmpty(Description);
        public DateTime Timestamp { get; private set; }
    }
}
