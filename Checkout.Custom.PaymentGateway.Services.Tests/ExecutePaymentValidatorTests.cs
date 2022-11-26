using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Checkout.Custom.Common.Model;
using Checkout.Custom.PaymentGateway.Services.Request;
using Checkout.Custom.PaymentGateway.Services.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Checkout.Custom.PaymentGateway.Services.Tests
{
    public class ExecutePaymentValidatorTests
    {
        private CardDetails cardDetails;
        private ExecutePaymentValidator validator;

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Should_Have_Error_When_Amount_Less_Than_Or_Equal_To_Zero(decimal amount)
        {
            Setup();
            var testValidationResult = validator.TestValidate(new ExecutePayment() { Amount = amount });
            testValidationResult.ShouldHaveValidationErrorFor(command => command.Amount);
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        public void Should_Have_Error_When_MerchantId_Is_Empty_Or_Null(Guid merchantId)
        {
            Setup();
            var testValidationResult = validator.TestValidate(new ExecutePayment() { MerchantId = merchantId });
            testValidationResult.ShouldHaveValidationErrorFor(command => command.MerchantId);
        }

        [Fact]
        public void Should_Have_Error_When_CardDetails_Is_Null()
        {
            Setup();
            var testValidationResult = validator.TestValidate(new ExecutePayment() { Amount = 19 });
            testValidationResult.ShouldHaveValidationErrorFor(command => command.CardDetails);
        }
        private void Setup()
        {
            validator = new ExecutePaymentValidator();
        }

    }
}
