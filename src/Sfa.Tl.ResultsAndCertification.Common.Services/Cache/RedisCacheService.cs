using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase();
        }        

        public async Task<T> GetAsync<T>(string key)
        {
            key = GenerateCacheKey<T>(key);

            IDatabase database = GetDatabase();
            var cachedValue = await database.StringGetAsync(key);
            return cachedValue.HasValue ? JsonConvert.DeserializeObject<T>(cachedValue) : default(T);
        }

        public async Task SetAsync<T>(string key, T item, CacheExpiryTime cacheExpiryTime = CacheExpiryTime.Small)
        {
            await SetCustomValueAsync(key, item, TimeSpan.FromMinutes((int)cacheExpiryTime));
        }

        private async Task SetCustomValueAsync<T>(string key, T customType, TimeSpan cacheTime)
        {
            if (customType == null) return;
            key = GenerateCacheKey<T>(key);
            IDatabase database = GetDatabase();
            await database.StringSetAsync(key, JsonConvert.SerializeObject(customType), cacheTime);
        }

        public async Task<bool> KeyExistsAsync<T>(string key)
        {
            key = GenerateCacheKey<T>(key);
            var database = GetDatabase();
            return await database.KeyExistsAsync(key);
        }

        public async Task RemoveAsync<T>(string key)
        {
            key = GenerateCacheKey<T>(key);
            var database = GetDatabase();
            await database.KeyDeleteAsync(key, CommandFlags.FireAndForget);
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
