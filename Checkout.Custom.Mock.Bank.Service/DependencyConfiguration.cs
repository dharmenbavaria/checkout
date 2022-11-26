using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.Mock.Bank.Service
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddBankService(this IServiceCollection services)
        {
            services.AddScoped<ITransactionAuthCodeService, TransactionAuthCodeService>();
            return services;
        }
    }
}
