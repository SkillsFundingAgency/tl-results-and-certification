using System;

namespace Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges
{
    public abstract class Range<T>
        where T : IComparable<T>
    {
        public T From { get; set; }

        public T To { get; set; }

        public bool Contains(T item)
        {
            return item.CompareTo(From) >= 0 && item.CompareTo(To) <= 0;
        }
    }
}