using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Checkout.Custom.PaymentGateway.Entity;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Custom.PaymentGateway.Data.Context
{
    public class PaymentDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private const string Sterling = "GBP";
        private const string Euro = "EUR";
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var merchant1 = "2769f5ea-1337-4f80-a1ba-35c35eded186";
            var merchant2 = "444998b1-10d1-498c-a1e1-8822cf08215b";
            var merchant3 = "a0a095c6-4056-41bb-aa4c-c1937252e964";
            var merchant4 = "f6a46846-5ca8-47ed-882b-f5d1a292c2ec";
            var merchant5 = "6c2b2591-dfa7-4981-9739-e6f39b486b59";

            var transactionId1 = "5ed79f45-e7fb-4944-aca1-46d0795512da";
            var transactionId2 = "b792055d-1172-4fa8-a659-e754a39f18dd";
            var transactionId3 = "33502eb0-5f48-4295-b428-e6117ef3ae86";
            var transactionId4 = "fa9056a3-4920-4f9c-ba3c-485d6d1f59f5";
            var transactionId5 = "ddc09ff7-6f82-4cd6-a46d-cbbb6ff087c8";

            builder.Entity<PaymentTransaction>().HasData(new PaymentTransaction(
                Guid.Parse(transactionId1),
                Guid.Parse(merchant1),
                100,
                "Emily Bavaria",
                "1234567890123412",
                "10000",
                string.Empty,
                Sterling));

            builder.Entity<PaymentTransaction>().HasData(new PaymentTransaction(
                Guid.Parse(transactionId2),
                Guid.Parse(merchant2),
                100,
                "Dharmen Bavaria",
                "223456789012342",
                "10000",
                string.Empty,
                Sterling));

            builder.Entity<PaymentTransaction>().HasData(new PaymentTransaction(
                Guid.Parse(transactionId3),
                Guid.Parse(merchant3),
                100,
                "Serge M",
                "3234567890123412",
                "10000",
                string.Empty,
                Sterling));

            builder.Entity<PaymentTransaction>().HasData(new PaymentTransaction(
                Guid.Parse(transactionId4),
                Guid.Parse(merchant4),
                100,
                "Paolo Genes",
                "4234567890123412",
                "10000",
                string.Empty,
                Sterling));

            builder.Entity<PaymentTransaction>().HasData(new PaymentTransaction(
                Guid.Parse(transactionId5),
                Guid.Parse(merchant5),
                100,
                "Lucas Elliot",
                "5234567890123412",
                "10000",
                string.Empty,
                Sterling));

        }
    }
}
