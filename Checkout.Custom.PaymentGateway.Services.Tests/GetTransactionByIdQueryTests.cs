using Checkout.Custom.PaymentGateway.Dto;
using Checkout.Custom.PaymentGateway.Services.Command;
using Checkout.Custom.PaymentGateway.Services.Request;
using Checkout.Custom.PaymentGateway.Services.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Checkout.Custom.PaymentGateway.Services.Tests
{
    public class GetTransactionByIdQueryTests
    {
        private GetTransactionById _request;
        private GetTransactionByIdQueryHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<IPaymentTransactionsService> _paymentTransactionService;
        private Mock<LoggerMock<GetTransactionByIdQueryHandler>> _logger;

        [Fact]
        public async Task Should_Contain_ServiceUnavailable_StatusCode_When_An_Exception_Throws_By_The_Service()
        {
            //Arange
            Setup();
            _paymentTransactionService.Setup(x => x.GetByTransactionIdAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.ServiceUnavailable.ToString(), result.StatusCode);
        }

        [Fact]
        public async Task Should_Contain_NotFound_StatusCode_When_A_Transaction_Could_Not_Be_Found_By_The_Service()
        {
            //Arange
            Setup();
            PaymentTransactionDto response = null;
            _paymentTransactionService.Setup(x => x.GetByTransactionIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(response));

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound.ToString(), result.StatusCode);
        }

        [Fact]
        public async Task Should_Contain_Successful_StatusCode_When_A_Successfully_Transaction_Could_Be_Found_By_The_Service()
        {
            //Arange
            Setup();
            var transactionId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var response = new PaymentTransactionDto(
                transactionId,
                merchantId,
                100,
                "Paolo Regoli",
                "1234567812345678",
                "Successful",
                string.Empty,
                "EUR");

            _request.Id = transactionId;

            _paymentTransactionService.Setup(x => x.GetByTransactionIdAsync(_request.Id))
                .Returns(Task.FromResult(response));

            _cardsService.Setup(x => x.Decrypt(response.CardNumber)).Returns(response.CardNumber);

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            Assert.NotNull(result);
            Assert.Equal("Successful", result.StatusCode);
        }

        private void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _paymentTransactionService = new Mock<IPaymentTransactionsService>();
            _logger = new Mock<LoggerMock<GetTransactionByIdQueryHandler>>();

            _request = new GetTransactionById();
            _handler = new GetTransactionByIdQueryHandler(
                _cardsService.Object,
                _paymentTransactionService.Object,
                _logger.Object);
        }
    }
}
