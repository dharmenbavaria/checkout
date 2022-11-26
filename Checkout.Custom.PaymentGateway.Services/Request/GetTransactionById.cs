using Checkout.Custom.Common.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services.Request
{
    public class GetTransactionById : IRequest<PaymentTransactionResponseVm>
    {
        public Guid Id { get; set; }
    }
}
