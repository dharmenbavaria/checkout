using Checkout.Custom.Common.Model;
using Checkout.Custom.PaymentGateway.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Custom.PaymentGateway.Services.Tests
{
    public class CardsServiceTests
    {
        private ICardsService _cardService;
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Validation_Should_Fail_For_A_Null_Or_Empty_Card_Number(string cardNumber)
        {
            //Arrange
            Setup();
            var cardDetails = new CardDetails
            {
                CardNumber = cardNumber
            };

            //Act
            var result = _cardService.Validate(cardDetails);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("hello")]
        [InlineData("123412341234")]
        public void Validation_Should_Fail_For_An_Invalid_Card_Number(string cardNumber)
        {
            //Arrange
            Setup();
            var cardDetails = new CardDetails
            {
                CardNumber = cardNumber
            };

            //Act
            var result = _cardService.Validate(cardDetails);

            //Assert
            Assert.False(result);
        }
        private void Setup()
        {
            _cardService = new CardsService();
        }

    }
}
