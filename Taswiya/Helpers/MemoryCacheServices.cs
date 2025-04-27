namespace ConnectChain.Helpers
{
    using Microsoft.Extensions.Caching.Memory;

    public interface ICacheService
    {
        public Task<T?> GetAsync<T>(string key);
        public Task SetAsync<T>(string key, T value, TimeSpan duration);


    }
    public class MemoryCacheService(IMemoryCache memoryCache) : ICacheService
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        public Task<T?> GetAsync<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan duration)
        {
            _memoryCache.Set(key, value, duration);
            return Task.CompletedTask;
        }
    }
}
