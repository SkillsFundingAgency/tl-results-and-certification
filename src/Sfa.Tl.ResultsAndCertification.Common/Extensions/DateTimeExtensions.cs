using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static DateTime? GetNthWeekdayOfMonth(DateTime dt, DayOfWeek dayOfWeek, Months? month = null, int nthWeek = 0)
        {
            var currentMonth = month == null ? dt.Month : (int)month.Value;
            var daysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(dt.Year, currentMonth)).Select(day => new DateTime(dt.Year, currentMonth, day));

            var weekdays = daysInMonth.Where(d => d.DayOfWeek == dayOfWeek).OrderBy(d => d.Day).Select(d => d).ToList();

            var index = nthWeek - 1;

            if (index >= 0 && index < weekdays.Count)
                return weekdays.ElementAt(index);
            else if (index <= 0 && weekdays.Any())
                return weekdays.Last();
            else
                return null;
        }

        public static bool IsLastWeekdayOfMonth(this DateTime dt, DayOfWeek dayOfWeek, Months month)
        {
            // Passing nthWeek = 0 will give the last record
            var nthDate = GetNthWeekdayOfMonth(dt, dayOfWeek, month);
            return nthDate != null && nthDate.Value.Date.Equals(dt.Date);
        }

        public static bool IsNthWeekdayOfMonth(this DateTime dt, DayOfWeek dayOfWeek, Months month, int nthWeek)
        {
            // Passing nthWeek = 0 will give the last record
            var nthDate = GetNthWeekdayOfMonth(dt, dayOfWeek, month, nthWeek);
            return nthDate != null && nthDate.Value.Date.Equals(dt.Date);
        }

        public static DateTime? GetNthDateOfMonth(this DateTime dt, DayOfWeek dayOfWeek, Months month, int nthWeek)
        {
            return GetNthWeekdayOfMonth(dt, dayOfWeek, month, nthWeek);
        }

        public static DateTime SubtractDays(this DateTime dt, int numberOfDays)
            => dt.AddDays(numberOfDays * -1);
    }
}