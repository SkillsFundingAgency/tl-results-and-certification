using System;
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

        public static TSource MinBy<TSource>(this IEnumerable<TSource> collection, Func<TSource, int> selector)
        {
            IEnumerable<TSource> sortedCollection = collection.OrderBy(selector);
            return sortedCollection.FirstOrDefault();
        }

        public static TSource MaxBy<TSource>(this IEnumerable<TSource> collection, Func<TSource, int> selector)
        {
            IEnumerable<TSource> sortedCollection = collection.OrderByDescending(selector);
            return sortedCollection.FirstOrDefault();
        }
    }
}