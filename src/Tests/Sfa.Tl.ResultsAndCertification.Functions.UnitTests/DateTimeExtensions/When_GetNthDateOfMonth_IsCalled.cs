using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.DateTimeExtensions
{
    public class When_GetNthDateOfMonth_IsCalled : TestSetup
    {
        [Theory]
        [MemberData(nameof(NthDateOfMonthData))]
        public void Then_Returns_Expected_Results(DateTime inputDate, DayOfWeek dayOfWeek, Months month, int nthWeek, DateTime? expectedResult)
        {
            var result = inputDate.GetNthDateOfMonth(dayOfWeek, month, nthWeek);
            result.Should().Be(expectedResult);
        }
    }
}
