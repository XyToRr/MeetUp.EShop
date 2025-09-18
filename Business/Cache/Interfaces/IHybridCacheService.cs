using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetUp.EShop.Business.Cache.Interfaces
{
    public interface IHybridCacheService
    {
        Task<T> GetCacheAsync<T>(string key, Func<Task<T>> acquire);
        Task RemoveCacheAsync(string key);
        Task SetCacheAsync<T>(string key, T value);
    }
}
