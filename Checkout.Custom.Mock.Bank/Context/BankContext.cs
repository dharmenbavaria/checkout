using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.Custom.Mock.Bank.Entity;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Custom.Mock.Bank.Data.Context
{
    public class BankContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public BankContext(DbContextOptions<BankContext> options)
            : base(options)
        {
        }
        public DbSet<TransactionAuthCode> TransactionAuthCodeCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TransactionAuthCode>().HasData(
                new TransactionAuthCode(Guid.NewGuid(), "05", "20005", "Declined - Do not honour"),
                new TransactionAuthCode(Guid.NewGuid(), "12", "20012", "Invalid transaction"),
                new TransactionAuthCode(Guid.NewGuid(), "14", "20014", "Invalid card number"),
                new TransactionAuthCode(Guid.NewGuid(), "51", "20051", "Insufficient funds"),
                new TransactionAuthCode(Guid.NewGuid(), "54", "20087", "Bad track data"),
                new TransactionAuthCode(Guid.NewGuid(), "62", "20062", "Restricted card"),
                new TransactionAuthCode(Guid.NewGuid(), "63", "20063", "Security violation"),
                new TransactionAuthCode(Guid.NewGuid(), "9998", "20068", "Response received too late / timeout"),
                new TransactionAuthCode(Guid.NewGuid(), "150", "20150", "Card not 3D Secure enabled")
                );
        }
    }
}
