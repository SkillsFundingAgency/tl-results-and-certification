using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private static readonly Lazy<MemoryCache> _cache = new Lazy<MemoryCache>(() => new MemoryCache(new MemoryCacheOptions()));
        
        public async Task<T> GetAsync<T>(string key)
        {
            key = GenerateCacheKey<T>(key);
            return await Task.FromResult(JsonConvert.DeserializeObject<T>(_cache.Value.Get<string>(key)));
        }

        public async Task<bool> KeyExistsAsync<T>(string key)
        {
            key = GenerateCacheKey<T>(key);
            return await Task.FromResult(_cache.Value.Get(key) != null);
        }

        public async Task RemoveAsync<T>(string key)
        {
            key = GenerateCacheKey<T>(key);
            _cache.Value.Remove(key);
            await Task.CompletedTask;
        }

        public async Task<T> GetAndRemoveAsync<T>(string key)
        {
            key = GenerateCacheKey<T>(key);
            var cacheValue = _cache.Value.Get<string>(key);
            _cache.Value.Remove(key);
            return await Task.FromResult(JsonConvert.DeserializeObject<T>(cacheValue));
        }

        public async Task SetAsync<T>(string key, T item, CacheExpiryTime cacheExpiryTime = CacheExpiryTime.Small)
        {
            key = GenerateCacheKey<T>(key);
            _cache.Value.Set(key, JsonConvert.SerializeObject(item), TimeSpan.FromHours((int)cacheExpiryTime));
            await Task.CompletedTask;
        }

        static string GenerateCacheKey<T>(string key)
        {
            return GenerateCacheKey(typeof(T), key);
        }

        static string GenerateCacheKey(Type objectType, string key)
        {
            return $"{key}:{objectType.Name}".ToLower();
        }        
    }
}
