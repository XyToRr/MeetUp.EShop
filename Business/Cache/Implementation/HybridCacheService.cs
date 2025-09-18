using MeetUp.EShop.Business.Cache.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Business.Cache.Implementation
{
    public class HybridCacheService : IHybridCacheService
    {
        private readonly HybridCache _cache;
        public HybridCacheService(HybridCache cache)
        {
            _cache = cache;
        }
        public async Task<T> GetCacheAsync<T>(string key,Func< Task<T>> acquire)
        {
            var result = await _cache.GetOrCreateAsync<T>(key,
                async cancel => await acquire(),
                new HybridCacheEntryOptions
                {
                    LocalCacheExpiration = TimeSpan.FromMinutes(1),
                    Expiration = TimeSpan.FromMinutes(5)
                });


            return result;
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task SetCacheAsync<T>(string key, T value)
        {
            await _cache.SetAsync<T>(key, value,
                new HybridCacheEntryOptions
                {
                    Expiration = TimeSpan.FromMinutes(5),
                    LocalCacheExpiration = TimeSpan.FromMinutes(1)
                });

        }
    }
}
