using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.Custom.Common.Repository;
using Checkout.Custom.PaymentGateway.Data.Context;
using Checkout.Custom.PaymentGateway.Entity;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Custom.PaymentGateway.Data.Repository
{
    public interface IPaymentTransactionRepository
        : IRepository<PaymentTransaction>
    {
        Task<List<PaymentTransaction>> GetTransactionsByMerchantId(Guid merchantId);
    }

    public class PaymentTransactionRepository : Repository<PaymentTransaction>, IPaymentTransactionRepository
    {
        public PaymentTransactionRepository(PaymentDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<List<PaymentTransaction>> GetTransactionsByMerchantId(Guid merchantId)
        {
            return await Query(x => x.MerchantId == merchantId).ToListAsync();
        }
    }
}
