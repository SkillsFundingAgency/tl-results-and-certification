using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers.CommonHelperTests
{
    public class When_GetPrsStatusDisplayText_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                var romm = "<strong class=\"govuk-tag govuk-tag--blue\">ROMM</strong>";
                var appeal = "<strong class=\"govuk-tag govuk-tag--purple\">Appeal</strong>";
                var final = "<strong class=\"govuk-tag govuk-tag--red\">Final</strong>";

                return new[]
                {
                    // Object params { PrsStatus, RommEndDate, AppealEndDate, ExpectedResult}
                    new object[] { null, null, null, final},
                    new object[] { PrsStatus.UnderReview, null, null, romm },
                    new object[] { PrsStatus.Reviewed, DateTime.Today.AddDays(1), DateTime.Today.AddDays(5), string.Empty },
                    new object[] { PrsStatus.Reviewed, DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-1), final },
                    new object[] { PrsStatus.BeingAppealed, null, null, appeal },
                    new object[] { PrsStatus.BeingAppealed, DateTime.Today.AddDays(-10), DateTime.Today.AddDays(1), appeal },
                    new object[] { PrsStatus.Final, null, null, final},
                    new object[] { null, null, DateTime.Today.AddDays(-1), final },
                    new object[] { null, null, DateTime.Today.AddDays(1), string.Empty}
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(PrsStatus? prsStatus, DateTime? rommEndDate, DateTime? appealEndDate, string expectedResult)
        {
            var actualResult = CommonHelper.GetPrsStatusDisplayText(prsStatus, rommEndDate, appealEndDate);
            actualResult.Should().Be(expectedResult);
        }
    }
}
