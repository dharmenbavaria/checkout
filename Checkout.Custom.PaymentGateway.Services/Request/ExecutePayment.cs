using Checkout.Custom.Common.Model;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services.Request
{
    public class ExecutePayment : IRequest<Guid>
    {
        public Guid MerchantId { get; set; }
        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
}
