using Checkout.Custom.PaymentGateway.Services.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services.Validators
{
    public class ExecutePaymentValidator : AbstractValidator<ExecutePayment>
    {
        private List<string> _supportedCurrency = new List<string>() { "GBP", "EUR" };
        public ExecutePaymentValidator()
        {
            RuleFor(v => v.Amount)
                .GreaterThan(0);
            RuleFor(v => v.Currency)
                .Must(x => _supportedCurrency.Contains(x))
                .WithMessage("Currency supported are: " + String.Join(",", _supportedCurrency));

            RuleFor(v => v.MerchantId)
                .NotNull()
                .NotEmpty()
                .NotEqual(Guid.Empty);
            RuleFor(v => v.CardDetails)
                .NotNull()
                .SetValidator(new CardDetailsValidator())
                .NotEmpty();
        }
    }
}
