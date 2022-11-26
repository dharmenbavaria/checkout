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
    public class GetTransactionByMerchantIdQueryTests
    {
        private GetTransactionByMerchantId _request;
        private GetTransactionByMerchantIdQueryHandler _handler;

        private Mock<ICardsService> _cardsService;
        private Mock<IPaymentTransactionsService> _paymentTransactionService;
        private Mock<LoggerMock<GetTransactionByMerchantIdQueryHandler>> _logger;
        

        [Fact]
        public async Task Should_Contain_Empty_Response_When_An_Exception_Throws_By_Service_By_The_Service()
        {
            //Arange
            Setup();
            _paymentTransactionService.Setup(x => x.GetByMerchantIdAsync(It.IsAny<Guid>()))
                .Throws(new Exception());

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_Contain_Empty_Response_When_Any_Transaction_Could_Be_Found_By_The_Given_MerchantId()
        {
            //Arange
            Setup();
            List<PaymentTransactionDto> response = null;
            _paymentTransactionService.Setup(x => x.GetByMerchantIdAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(response));

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_Contain_A_Not_Empty_When_At_Least_One_Transaction_Could_Be_Found_By_The_Given_MerchantId()
        {
            //Arange
            Setup();
            var transactionId = Guid.NewGuid();
            var merchantId = Guid.NewGuid();
            var response = new List<PaymentTransactionDto>
            {
                new PaymentTransactionDto(
                    transactionId,
                    merchantId,
                    100,
                    "Paolo Regoli",
                    "1234567812345678",
                    "Successful",
                    string.Empty,
                    "EUR")
            };

            _request.MerchantId = merchantId;

            _paymentTransactionService.Setup(x => x.GetByMerchantIdAsync(_request.MerchantId))
                .Returns(Task.FromResult(response));

            _cardsService.Setup(x => x.Decrypt(It.IsAny<string>())).Returns("1234567812345678");

            //Act
            var result = await _handler.Handle(_request, new CancellationToken());

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(transactionId, result.FirstOrDefault().TransactionId );
        }

        private void Setup()
        {
            _cardsService = new Mock<ICardsService>();
            _paymentTransactionService = new Mock<IPaymentTransactionsService>();
            _logger = new Mock<LoggerMock<GetTransactionByMerchantIdQueryHandler>>();

            _request = new GetTransactionByMerchantId();
            _handler = new GetTransactionByMerchantIdQueryHandler(
                _cardsService.Object,
                _paymentTransactionService.Object,
                _logger.Object);
        }
    }
}
