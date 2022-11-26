using Checkout.Custom.Common.Repository;
using Checkout.Custom.Mock.Bank.Data.Context;
using Checkout.Custom.Mock.Bank.Entity;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Custom.Mock.Bank.Data.Repository
{
    public interface ITransactionAuthCodeRepository : IRepository<TransactionAuthCode>
    {
        Task<TransactionAuthCode?> ValidateAsync(decimal amount);
    }

    public class TransactionAuthCodeRepository : Repository<TransactionAuthCode>, ITransactionAuthCodeRepository
    {
        private readonly BankContext _bankContext;

        public TransactionAuthCodeRepository(BankContext bankContext) : base(bankContext)
        {
            _bankContext = bankContext;
        }

        public async Task<TransactionAuthCode?> ValidateAsync(decimal amount)
        {
            return await Query(x => amount.ToString().EndsWith(x.CardEndWith)).FirstOrDefaultAsync();
        }
    }
}
