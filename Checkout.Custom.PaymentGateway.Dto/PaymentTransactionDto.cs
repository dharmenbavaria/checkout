namespace Checkout.Custom.PaymentGateway.Dto
{
    public class PaymentTransactionDto
    {
        public PaymentTransactionDto()
        {
        }

        public PaymentTransactionDto(
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

        public Guid Id { get; set; }
        public Guid MerchantId { get; set; }
        public string Currency { get; set; }
    
        public decimal Amount { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; private set; }
    }
}