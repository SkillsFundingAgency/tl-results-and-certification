using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.DateTimeExtensions
{
    public class When_IsNthWeekdayOfMonth_IsCalled : TestSetup
    {
        [Theory]
        [MemberData(nameof(NthWeekdayOfMonthData))]
        public void Then_Returns_Expected_Results(DateTime inputDate, DayOfWeek dayOfWeek, Months month, int nthWeek, bool isValid)
        {
            var result = inputDate.IsNthWeekdayOfMonth(dayOfWeek, month, nthWeek);

            result.Should().Be(isValid);
        }
    }
}
