using Checkout.Custom.Common.Model;
using Checkout.Custom.PaymentGateway.Services.Validators;
using Xunit;
using FluentValidation.TestHelper;

namespace Checkout.Custom.PaymentGateway.Services.Tests
{
    public class CardDetailsValidatorTests
    {
        private CardDetailsValidator validator;

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Have_Error_When_CardHolderName_Is_Null_Or_Empty(string cardHolderName)
        {
            Setup();
            var validation = validator.TestValidate(new CardDetails() { CardHolderName = cardHolderName });
            validation.ShouldHaveValidationErrorFor(x => x.CardHolderName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1234")]
        [InlineData("12")]
        [InlineData("hello")]
        public void Should_Have_Error_When_Cvv_Is_Null_Or_Empty_Or_Not_A_Number_Or_It_Is_Not_Compliant_With_The_Required_Length(string cvv)
        {
            Setup();
            var validation = validator.TestValidate(new CardDetails() { Cvv = cvv });
            validation.ShouldHaveValidationErrorFor(x => x.Cvv);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1234")]
        [InlineData("12")]
        [InlineData("hello")]
        public void Should_Have_Error_When_CardNumber_Is_Null_Or_Empty_Or_It_Is_Not_Compliant_With_The_Required_Length(string cardNumber)
        {
            Setup();
            var validation = validator.TestValidate(new CardDetails() { CardNumber = cardNumber });
            validation.ShouldHaveValidationErrorFor(x => x.CardNumber);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("111")]
        [InlineData("hello")]
        public void Should_Have_Error_When_ExpirationMonth_Is_Null_Or_Empty_Or_Not_A_Number_Or_It_Is_Not_Compliant_With_The_Required_Length(string expirationMonth)
        {
            Setup();
            var validation = validator.TestValidate(new CardDetails() { ExpirationMonth = expirationMonth });
            validation.ShouldHaveValidationErrorFor(x => x.ExpirationMonth);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("111")]
        [InlineData("hello")]
        public void Should_Have_Error_When_ExpirationYear_Is_Null_Or_Empty_Or_Not_A_Number_Or_It_Is_Not_Compliant_With_The_Required_Length(string expirationYear)
        {
            Setup();
            var validation = validator.TestValidate(new CardDetails() { ExpirationYear = expirationYear });
            validation.ShouldHaveValidationErrorFor(x => x.ExpirationYear);
        }
        private void Setup()
        {
            validator = new CardDetailsValidator();
        }

    }
}
