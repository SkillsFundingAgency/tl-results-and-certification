using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static DateTime? GetNthWeekdayOfMonth(DateTime dt, DayOfWeek dayOfWeek, int nthWeek = 0)
        {
            var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, dt.Month)).Select(day => new DateTime(dt.Year, dt.Month, day));

            var weekdays = daysInMonth.Where(d => d.DayOfWeek == dayOfWeek).OrderBy(d => d.Day).Select(d => d).ToList();

            var index = nthWeek - 1;

            if (index >= 0 && index < weekdays.Count)
                return weekdays.ElementAt(index);
            else if (index <= 0 && weekdays.Any())
                return weekdays.Last();
            else
                return null;
        }

        public static bool IsLastWeekdayOfMonth(this DateTime dt, DayOfWeek dayOfWeek)
        {
            // Passing nthWeek = 0 will give the last record
            var nthDate = GetNthWeekdayOfMonth(dt, dayOfWeek);
            return nthDate != null && nthDate.Value.Date.Equals(dt.Date);
        }
    }
}
