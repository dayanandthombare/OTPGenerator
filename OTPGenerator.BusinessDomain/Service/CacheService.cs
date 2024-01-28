using OTPGenerator.BusinessDomain.Interface;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace OTPGenerator.BusinessDomain.Service
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set(string key, string value, DateTime expiryTime)
        {
            _memoryCache.Set(key, value, expiryTime);
        }
    }

}
