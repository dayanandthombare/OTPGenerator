using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using OTPGenerator.Server.Controllers;
using OTPGenerator.BusinessDomain.Interface;

namespace OTPGeneratorTest
{
    [TestFixture]
    public class OtpControllerTests
    {
        private Mock<IOtpService>? _otpServiceMock;
        private Mock<ICacheService>? _cacheServiceMock;
        private OtpController? _controller;

        [SetUp]
        public void Setup()
        {
            _otpServiceMock = new Mock<IOtpService>();
            _cacheServiceMock = new Mock<ICacheService>();
            _controller = new OtpController(_otpServiceMock.Object, _cacheServiceMock.Object);
        }

        [Test]
        public void GenerateOtp_ReturnOtpWithExpiryTime()
        {
            
            var userId = "test_user";
            _otpServiceMock?.Setup(s => s.GenerateOtp()).Returns(It.IsAny<string>());
            _cacheServiceMock?.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()));            
            var actionResult = _controller?.GenerateOtp(new OtpRequest { UserId = userId });            
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult);
            dynamic? response = okResult?.Value;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response?.Otp);
            Assert.IsInstanceOf<string>(response?.Otp);
            Assert.IsFalse(string.IsNullOrEmpty(response?.Otp));
            Assert.IsTrue(response?.ValidUntil >= DateTime.UtcNow && response?.ValidUntil <= DateTime.UtcNow.AddSeconds(30));
        }

        [Test]
        public void GenerateOtp_StoreOtpInCache()
        {
            
            var userId = "test_user";
            string? capturedUserId = null;
            string? capturedOtp = null;
            DateTime? capturedExpiryTime = null;
            _otpServiceMock?.Setup(s => s.GenerateOtp()).Returns(It.IsAny<string>());
            _cacheServiceMock?.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()))
                             .Callback<string, string, DateTime>((id, otp, expiryTime) =>
                             {
                                 capturedUserId = id;
                                 capturedOtp = otp;
                                 capturedExpiryTime = expiryTime;
                             });            
            _controller?.GenerateOtp(new OtpRequest { UserId = userId });            
            Assert.IsNotNull(capturedUserId);
            Assert.IsNotNull(capturedOtp);
            Assert.IsNotNull(capturedExpiryTime);
            Assert.AreEqual(userId, capturedUserId);
            Assert.IsFalse(string.IsNullOrEmpty(capturedOtp));
            Assert.IsTrue(capturedExpiryTime >= DateTime.UtcNow && capturedExpiryTime <= DateTime.UtcNow.AddSeconds(30));
        }


    }
    public class OtpResponse
    {
        public string? Otp { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
