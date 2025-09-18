using MeetUp.EShop.Business.Cache.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Business.Cache.Implementation
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetCacheAsync<T>(string key, T value)
        {
            var serializedValue = System.Text.Json.JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedValue, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var cachedValue = await _cache.GetStringAsync(key);
            if (cachedValue == null)
            {
                return default;
            }
            return System.Text.Json.JsonSerializer.Deserialize<T>(cachedValue);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
