using Checkout.Custom.Common.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services.Request
{
    public class GetTransactionByMerchantId : IRequest<List<PaymentTransactionResponseVm>>
    {
        public Guid MerchantId { get; set; }
    }
}
