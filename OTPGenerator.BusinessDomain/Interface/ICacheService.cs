namespace OTPGenerator.BusinessDomain.Interface
{
    public interface ICacheService
    {
        void Set(string key, string value, DateTime expiryTime);
    }
}
