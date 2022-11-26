using System.ComponentModel.DataAnnotations;
using Checkout.Custom.Common.Entity;

namespace Checkout.Custom.Mock.Bank.Entity
{
    public class TransactionAuthCode : IEntity
    {
        public TransactionAuthCode(
            Guid id,
            string cardEndWith,
            string transactionCode,
            string description)
        {
            Id = id;
            CardEndWith = cardEndWith;
            TransactionCode = transactionCode;
            Description = description;
        }

        [Key]
        public Guid Id { get; set; }
        public string CardEndWith { get; set; }
        public string TransactionCode { get; set; }
        public string Description { get; set; }
    }
}
