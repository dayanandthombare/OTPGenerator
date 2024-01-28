using Microsoft.AspNetCore.Mvc;
using OTPGenerator.BusinessDomain.Interface;

namespace OTPGenerator.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;
        private readonly ICacheService _cacheService;

        public OtpController(IOtpService otpService, ICacheService cacheService)
        {
            _otpService = otpService;
            _cacheService = cacheService;
        }

        [HttpPost("generate")]
        public ActionResult GenerateOtp([FromBody] OtpRequest request)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            var expiryTime = DateTime.UtcNow.AddSeconds(30);

            _cacheService.Set(request.UserId, otp, expiryTime);

            return Ok(new OtpResponse { Otp = otp, ValidUntil = expiryTime });
        }
    }
    public class OtpRequest
    {
        public required string UserId { get; set; }
    }
    public class OtpResponse
    {
        public string? Otp { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
