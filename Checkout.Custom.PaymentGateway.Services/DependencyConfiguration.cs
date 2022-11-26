using AutoMapper;
using Checkout.Custom.Mock.Bank.Service;
using Checkout.Custom.PaymentGateway.Services.Behaviours;
using Checkout.Custom.PaymentGateway.Services.Mapping;
using Checkout.Custom.PaymentGateway.Services.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services
{
    public static class DependencyConfiguration
    {
        public static IServiceCollection AddBusinessConfiguration(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<ICardsService, CardsService>();
            services.AddScoped<IPaymentTransactionsService, PaymentTransactionsService>();

            return services;
        }

    }
}
