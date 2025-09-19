using MeetUp.EShop.Business.Cache.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MeetUp.EShop.Tests.TestServices
{
    public class TestCacheService : IHybridCacheService
    {
        private readonly ConcurrentDictionary<string, object?> _cache = new();

        public Task<T> GetCacheAsync<T>(string key, Func<Task<T>> acquire)
        {
            if (_cache.TryGetValue(key, out var existing) && existing is T typed)
            {
                return Task.FromResult(typed);
            }

            return AcquireAndStore(key, acquire);
        }

        public Task RemoveCacheAsync(string key)
        {
            _cache.TryRemove(key, out _);
            return Task.CompletedTask;
        }

        public Task SetCacheAsync<T>(string key, T value)
        {
            _cache[key] = value!;
            return Task.CompletedTask;
        }

        private async Task<T> AcquireAndStore<T>(string key, Func<Task<T>> acquire)
        {
            var value = await acquire();
            _cache[key] = value!;
            return value;
        }
    }
}
