using System;
using System.Net;
using System.Threading.Tasks;
using Checkout.Custom.Common.Model;
using Checkout.Custom.Mock.Bank.Data.Repository;
using Checkout.Custom.Mock.Bank.Entity;
using Checkout.Custom.Mock.Bank.Service.Model;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;

namespace Checkout.Custom.Mock.Bank.Service.Tests
{
    public class TransactionAuthCodeServiceTests
    {
        private TransactionAuthRequest _transactionAuthRequest;
        private ITransactionAuthCodeService _transactionAuthCodeService;

        private Mock<ITransactionAuthCodeRepository> _transactionAuthCodeRepository;
        private Mock<LoggerMock<TransactionAuthCodeService>> _logger;

        [Fact]
        public async Task For_A_Given_Zero_Amount_A_NotAcceptable_StatusCode_From_Response_Is_Expected()
        {
            //Arange
            Setup();
            _transactionAuthRequest.Amount = 0;

            //Act
            var result = await _transactionAuthCodeService.VerifyAsync(_transactionAuthRequest);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotAcceptable.ToString(), result.Code);
        }

        [Fact]
        public async Task For_A_Given_Exception_Thrown_By_Repository_A_ServiceUnavailable_StatusCode_From_Response_Is_Expected()
        {
            //Arange
            Setup();
            _transactionAuthRequest.Amount = 100;

            _transactionAuthCodeRepository.Setup(x => x.ValidateAsync(It.IsAny<decimal>()))
                .Throws(new Exception());

            //Act
            var result = await _transactionAuthCodeService.VerifyAsync(_transactionAuthRequest);

            //Assert
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.ServiceUnavailable.ToString(), result.Code );
        }

        [Fact]
        public async Task Should_Return_A_Successful_StatusCode_When_A_Null_Response_By_Repository()
        {
            //Arange
            Setup();
            _transactionAuthRequest.Amount = 100;

            TransactionAuthCode response = null;
            _transactionAuthCodeRepository.Setup(x => x.ValidateAsync(It.IsAny<decimal>()))
                .ReturnsAsync(response);

            //Act
            var result = await _transactionAuthCodeService.VerifyAsync(_transactionAuthRequest);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("10000", result.Code);
        }

        [Fact]
        public async Task Should_Return_A_Not_Successful_StatusCode_When_A_Not_Null_Null_Response_By_Repository()
        {
            //Arange
            Setup();
            _transactionAuthRequest.Amount = 105;

            var response = new TransactionAuthCode(Guid.NewGuid(), "05", "20005", "Declined - Do not honour");
            _transactionAuthCodeRepository.Setup(x => x.ValidateAsync(It.IsAny<decimal>()))
                .ReturnsAsync(response);

            //Act
            var result = await _transactionAuthCodeService.VerifyAsync(_transactionAuthRequest);

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual("10000", result.Code );
            Assert.Equal(response.TransactionCode, result.Code);
        }
        private void Setup()
        {
            _transactionAuthCodeRepository = new Mock<ITransactionAuthCodeRepository>();
            _logger = new Mock<LoggerMock<TransactionAuthCodeService>>();

            _transactionAuthRequest = new TransactionAuthRequest(new CardDetails(), 100);
            _transactionAuthCodeService = new TransactionAuthCodeService(
                _transactionAuthCodeRepository.Object,
                _logger.Object);
        }


    }
}