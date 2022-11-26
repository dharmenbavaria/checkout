using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.Custom.Common.Model;

namespace Checkout.Custom.Mock.Bank.Service.Model
{
    public class TransactionAuthRequest
    {
        public TransactionAuthRequest(
            CardDetails cardDetails,
            decimal amount)
        {
            CardDetails = cardDetails;
            Amount = amount;
        }

        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
    }
}
