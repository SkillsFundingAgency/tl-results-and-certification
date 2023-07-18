using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }
    }
}