using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers.CommonHelperTests
{
    public class When_IsAppealsAllowed_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Object params { AppealEndDate, ExpectedResult}
                    new object[] { null, false},
                    new object[] { DateTime.Today, true},
                    new object[] { DateTime.Today.AddDays(1), true},
                    new object[] { DateTime.Today.AddDays(-1), false},
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(DateTime? appealEndDate, bool expectedResult)
        {
            var actualResult = CommonHelper.IsAppealsAllowed(appealEndDate);
            actualResult.Should().Be(expectedResult);
        }
    }
}
