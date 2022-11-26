using Checkout.Custom.Common.Model;
using Checkout.Custom.Mock.Bank.Service;
using Checkout.Custom.Mock.Bank.Service.Model;
using Checkout.Custom.PaymentGateway.Services.Command;
using Checkout.Custom.PaymentGateway.Services.Request;
using Checkout.Custom.PaymentGateway.Services.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Custom.PaymentGateway.Services.Tests
{
    public class ExecutePaymentTests
    {
        private ExecutePayment _command;
        private ExecutePaymentHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<ITransactionAuthCodeService> _transactionAuthCodeService;
        private Mock<IMediator> _mediator;
        private Mock<LoggerMock<ExecutePaymentHandler>> _logger;

        
       
        [Fact]
        public async Task Should_Return_A_TransactionId_When_An_Exception_Thrown_By_AuthCodeProvider()
        {
            Setup();
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(true);
            _transactionAuthCodeService.Setup(x => x.VerifyAsync(It.IsAny<TransactionAuthRequest>()))
                .Throws(new Exception());

            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            
            Assert.NotEqual(result, Guid.Empty);
        }

        [Fact]
        public async Task Should_Return_A_TransactionId_When_An_Invalid_Card_Data()
        {
            Setup();
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(false);

            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _transactionAuthCodeService.Verify(x => x.VerifyAsync(It.IsAny<TransactionAuthRequest>()), Times.Never);
            
            Assert.NotEqual(result, Guid.Empty);
        }

        [Theory]
        [InlineData("20005", "Declined - Do not honour", false)]
        [InlineData("20012", "Invalid transaction", false)]
        [InlineData("20063", "Security violation", false)]
        [InlineData("Successful", "", true)]
        public async Task Should_Return_A_TransactionId_When_The_Auth_Verify_The_Transaction(
            string statusCode, string description, bool verified)
        {
            Setup();
            //Arange
            _cardsService.Setup(x => x.Validate(It.IsAny<CardDetails>())).Returns(true);

            var transactionId = Guid.NewGuid();
            var transactionAuthResponse = new TransactionAuthResponse(transactionId, verified, statusCode, description);
            _transactionAuthCodeService.Setup(x => x.VerifyAsync(It.IsAny<TransactionAuthRequest>()))
                .Returns(Task.FromResult(transactionAuthResponse));
            //Act
            var result = await _handler.Handle(_command, new CancellationToken());

            //Assert
            _transactionAuthCodeService.Verify(x => x.VerifyAsync(It.IsAny<TransactionAuthRequest>()), Times.Exactly(1));
            Assert.NotEqual(result, Guid.Empty);
        }

        private void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _transactionAuthCodeService = new Mock<ITransactionAuthCodeService>();
            _mediator = new Mock<IMediator>();
            _logger = new Mock<LoggerMock<ExecutePaymentHandler>>();

            _command = new ExecutePayment
            {
                Amount = 100,
                CardDetails = new CardDetails(),
                MerchantId = Guid.NewGuid()
            };

            _handler = new ExecutePaymentHandler(
                _cardsService.Object,
                _transactionAuthCodeService.Object,
                _mediator.Object,
                _logger.Object);
        }

    }
}