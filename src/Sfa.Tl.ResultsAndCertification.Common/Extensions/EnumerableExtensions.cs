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

        public static bool ContainsSingle<T>(this IEnumerable<T> collection)
        {
            return !collection.IsNullOrEmpty() && collection.Count() == 1;
        }

        public static TSource MinBy<TSource>(this IEnumerable<TSource> collection, Func<TSource, int> selector)
        {
            return ByOrderFunc(() => collection.OrderBy(selector));
        }

        public static TSource MaxBy<TSource>(this IEnumerable<TSource> collection, Func<TSource, int> selector)
        {
            return ByOrderFunc(() => collection.OrderByDescending(selector));
        }

        private static TSource ByOrderFunc<TSource>(Func<IEnumerable<TSource>> orderByFunc)
        {
            IEnumerable<TSource> sortedCollection = orderByFunc();
            return sortedCollection.FirstOrDefault();
        }
    }
}