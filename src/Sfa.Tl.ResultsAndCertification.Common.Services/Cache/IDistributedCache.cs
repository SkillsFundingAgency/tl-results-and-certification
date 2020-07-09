using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Cache
{
    public interface IDistributedCache
    {
        Task<T> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T item, CacheExpiryTime cacheExpiryTime = CacheExpiryTime.Small);

        Task RemoveAsync<T>(string key);

        Task<bool> KeyExistsAsync<T>(string key);
    }
}
