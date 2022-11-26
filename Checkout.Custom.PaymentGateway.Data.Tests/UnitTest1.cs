using Xunit;

namespace Checkout.Custom.PaymentGateway.Data.Tests
{
    public class PaymentTransactionRepositoryTest
    {
        [Fact]
        public void GetTransactionsByMerchantId_WithValidData_ProvidesPaymentTransaction()
        {
            new PaymentContext();
        }
    }
}