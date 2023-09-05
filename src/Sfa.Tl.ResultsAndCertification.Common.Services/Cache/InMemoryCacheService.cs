using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Cache
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly MemoryCache _cache = new(new MemoryCacheOptions());

        public Task<T> GetAsync<T>(string key)
        {
            string cachedValue = GetCachedValue<T>(key);
            return DeserializeOrDefaultAsync<T>(cachedValue);
        }

        public Task<bool> KeyExistsAsync<T>(string key)
        {
            string cachedValue = GetCachedValue<T>(key);
            return Task.FromResult(cachedValue != null);
        }

        public Task RemoveAsync<T>(string key)
        {
            string generatedCacheKey = GenerateCacheKey<T>(key);
            _cache.Remove(generatedCacheKey);

            return Task.CompletedTask;
        }

        public Task<T> GetAndRemoveAsync<T>(string key)
        {
            var cachedValue = GetCachedValue<T>(key, out string generatedCacheKey);
            _cache.Remove(generatedCacheKey);

            return DeserializeOrDefaultAsync<T>(cachedValue);
        }

        public Task SetAsync<T>(string key, T item, CacheExpiryTime cacheExpiryTime = CacheExpiryTime.Small)
        {
            string generatedCacheKey = GenerateCacheKey<T>(key);
            _cache.Set(generatedCacheKey, JsonConvert.SerializeObject(item), TimeSpan.FromHours((int)cacheExpiryTime));

            return Task.CompletedTask;
        }

        private string GetCachedValue<T>(string key)
        {
            return GetCachedValue<T>(key, out _);
        }

        private string GetCachedValue<T>(string key, out string generatedCacheKey)
        {
            generatedCacheKey = GenerateCacheKey<T>(key);
            return _cache.Get<string>(generatedCacheKey);
        }

        private string GenerateCacheKey<T>(string key)
        {
            return $"{key}:{typeof(T).Name}".ToLower();
        }

        private Task<T> DeserializeOrDefaultAsync<T>(string value)
        {
            T result = string.IsNullOrEmpty(value) ? default : JsonConvert.DeserializeObject<T>(value);
            return Task.FromResult(result);
        }
    }
}