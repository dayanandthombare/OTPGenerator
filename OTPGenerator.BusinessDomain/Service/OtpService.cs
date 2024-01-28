using OTPGenerator.BusinessDomain.Interface;

namespace OTPGenerator.BusinessDomain.Service
{
    public class OtpService : IOtpService
    {
        public string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}
