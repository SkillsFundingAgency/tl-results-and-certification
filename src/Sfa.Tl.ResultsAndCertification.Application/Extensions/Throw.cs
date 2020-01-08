using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Extensions
{
    public static class Throw
    {
        public static void IfNull<T>(T argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
        }
        public static void IfNullGuid(Guid argument, string name)
        {
            if (argument == Guid.NewGuid())
                throw new ArgumentException($"{name} cannot be null or empty", name);

        }
        public static void IfNullOrEmpty(string argument, string name)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentException($"{name} cannot be null or empty.", name);
        }

        public static void IfNullOrEmpty<T>(IEnumerable<T> argument, string name)
        {
            if (argument == null || !argument.Any())
                throw new ArgumentException($"{name} cannot be null or empty.", name);
        }

        public static void IfNullOrWhiteSpace(string argument, string name)
        {
            if (string.IsNullOrWhiteSpace(argument))
                throw new ArgumentException($"{name} cannot be null or empty or whitespace.", name);
        }

        public static void IfLessThan(int limit, int argument, string name)
        {
            if (argument < limit)
                throw new ArgumentOutOfRangeException($"{name} cannot be less than {limit}.", name);
        }

        public static void IfGreaterThan(int limit, int argument, string name)
        {
            if (argument > limit)
                throw new ArgumentOutOfRangeException($"{name} cannot be greater than {limit}.", name);
        }

        public static void IfLessThan(decimal limit, decimal argument, string name)
        {
            if (argument < limit)
                throw new ArgumentOutOfRangeException($"{name} cannot be less than {limit}.", name);
        }

        public static void IfGreaterThan(decimal limit, decimal argument, string name)
        {
            if (argument > limit)
                throw new ArgumentOutOfRangeException($"{name} cannot be greater than {limit}.", name);
        }
    }
}
