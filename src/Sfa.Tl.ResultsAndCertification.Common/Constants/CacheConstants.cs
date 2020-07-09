using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Constants
{
    public static class CacheConstants
    {
        public static TimeSpan SmallCacheTime = new TimeSpan(0, 0, 30, 0);
        public static TimeSpan MediumCacheTime = new TimeSpan(0, 0, 60, 0);
        public static TimeSpan LargeCacheTime = new TimeSpan(0, 0, 20, 0);

        public const string RegistrationCacheKey = "Registration";
    }
}
