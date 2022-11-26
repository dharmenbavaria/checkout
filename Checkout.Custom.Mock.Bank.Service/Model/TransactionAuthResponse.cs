using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.Mock.Bank.Service.Model
{
    public class TransactionAuthResponse
    {
        public TransactionAuthResponse(
            Guid transactionId,
            bool verified,
            string code,
            string description)
        {
            TransactionId = transactionId;
            Verified = verified;
            Code = code;
            Description = description;
        }

        public Guid TransactionId { get; set; }
        public bool Verified { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
