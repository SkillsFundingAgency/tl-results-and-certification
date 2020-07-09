namespace Sfa.Tl.ResultsAndCertification.Common.Helpers
{
    public class CacheKeyHelper
    {
        public static string GetCacheKey(string userId, string key)
        {
            return $"UserId:{userId}:{key}";
        }
    }
}
