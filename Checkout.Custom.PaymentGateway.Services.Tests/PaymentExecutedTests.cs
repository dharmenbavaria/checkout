using Checkout.Custom.PaymentGateway.Dto;
using Checkout.Custom.PaymentGateway.Services.Events;
using Checkout.Custom.PaymentGateway.Services.Request;
using Checkout.Custom.PaymentGateway.Services.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Custom.PaymentGateway.Services.Tests
{
    public class PaymentExecutedTests
    {
        private PaymentExecuted _event;
        private PaymentExecutedHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<IPaymentTransactionsService> _paymentTransactionsService;
        private Mock<LoggerMock<PaymentExecutedHandler>> _logger;

        [Fact]
        public async Task Should_Log_An_Error_When_An_Exception_Throws()
        {
            Setup();
            //Arange
            _paymentTransactionsService.Setup(x => x.AddAsync(It.IsAny<PaymentTransactionDto>()))
                .Throws(new Exception("test"));

            //Act + Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(_event, new CancellationToken()));
            Assert.Equal("test", ex.Message);
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Should_Not_Throw_An_Exception_When_The_Service_Return_A_Response()
        {
            Setup();
            //Arange
            _paymentTransactionsService.Setup(x => x.AddAsync(It.IsAny<PaymentTransactionDto>()))
                .Returns(Task.FromResult(new PaymentTransactionDto()));

            //Act
            await _handler.Handle(_event, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
        }
        private void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _paymentTransactionsService = new Mock<IPaymentTransactionsService>();
            _logger = new Mock<LoggerMock<PaymentExecutedHandler>>();

            _event = new PaymentExecuted
                (
                Guid.NewGuid(),
                Guid.NewGuid(),
                100,
                "Paolo Regoli",
                "2222333344445555",
                "Successful",
                string.Empty,
                true,
                "GBP");

            _handler = new PaymentExecutedHandler(
                _paymentTransactionsService.Object,
                _cardsService.Object,
                _logger.Object);
        }
    }
}
