using Checkout.Custom.Common.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.Custom.PaymentGateway.Services.Validators
{
    public class CardDetailsValidator : AbstractValidator<CardDetails>
    {
        public CardDetailsValidator()
        {
            RuleFor(v => v.CardHolderName)
                .NotNull()
                .NotEmpty();
            RuleFor(v => v.Cvv)
                .NotNull()
                .NotEmpty()
                .Must(x => int.TryParse(x, out _))
                .Length(3);
            RuleFor(v => v.CardNumber)
                .CreditCard()
                .NotNull()
                .NotEmpty()
                .Length(16);
            RuleFor(v => v.ExpirationMonth)
                .NotNull()
                .NotEmpty()
                .Must(x => int.TryParse(x, out _))
                .Length(2);
            RuleFor(v => v.ExpirationYear)
                .NotNull()
                .NotEmpty()
                .Must(x => int.TryParse(x, out _))
                .Length(4);
        }
    }
}
