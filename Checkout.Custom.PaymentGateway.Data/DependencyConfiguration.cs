using Checkout.Custom.PaymentGateway.Data.Context;
using Checkout.Custom.PaymentGateway.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Data
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddPaymentData(this IServiceCollection services)
        {
            services.AddDbContext<PaymentDbContext>(opt =>
               opt.UseInMemoryDatabase("Payment"));

            services.AddTransient<IPaymentTransactionRepository, PaymentTransactionRepository>();
          
            return services;
        }
    }
}
