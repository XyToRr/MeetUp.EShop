namespace MeetUp.EShop.Business.Cache.Interfaces
{
    public interface ICacheService
    {
        Task<T> GetCacheAsync<T>(string key);
        Task SetCacheAsync<T>(string key, T value);
        Task RemoveCacheAsync(string key);
    }
}