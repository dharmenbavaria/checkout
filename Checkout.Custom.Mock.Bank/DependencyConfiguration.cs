using Checkout.Custom.Mock.Bank.Data.Context;
using Checkout.Custom.Mock.Bank.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.Custom.Mock.Bank.Data
{

    public static class DependencyConfiguration
    {
        public static IServiceCollection AddBankData(this IServiceCollection services)
        {
            services.AddDbContext<BankContext>(opt =>
               opt.UseInMemoryDatabase("Bank"));

            services.AddTransient<ITransactionAuthCodeRepository, TransactionAuthCodeRepository>();

            return services;
        }
    }

}
