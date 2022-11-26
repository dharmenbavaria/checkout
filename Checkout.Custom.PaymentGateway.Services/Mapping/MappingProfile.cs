using AutoMapper;
using Checkout.Custom.PaymentGateway.Dto;
using Checkout.Custom.PaymentGateway.Entity;
using Checkout.Custom.PaymentGateway.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapTransaction();
        }

        private void MapTransaction()
        {
            CreateMap<PaymentTransaction, PaymentTransactionDto>();
            CreateMap<PaymentTransactionDto, PaymentTransaction>();
        }
    }
}
