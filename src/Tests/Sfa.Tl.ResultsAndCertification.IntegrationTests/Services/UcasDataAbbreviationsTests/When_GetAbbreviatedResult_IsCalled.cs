using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.UcasDataAbbreviationsTests
{
    public class When_GetAbbreviatedResult_IsCalled
    {
        private string _actualResult;

        public void When(UcasResultType ucasResultType, string result)
        {
            _actualResult = UcasDataAbbreviations.GetAbbreviatedResult(ucasResultType, result);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Expected_Results_Are_Returned(UcasResultType ucasResultType, string result, string expectedResult)
        {
            When(ucasResultType, result);
            _actualResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Pathway Result
                    new object[] { UcasResultType.PathwayResult, "A*", "A*" },
                    new object[] { UcasResultType.PathwayResult, "A", "A" },
                    new object[] { UcasResultType.PathwayResult, "B", "B" },
                    new object[] { UcasResultType.PathwayResult, "C", "C" },
                    new object[] { UcasResultType.PathwayResult, "D", "D" },
                    new object[] { UcasResultType.PathwayResult, "E", "E" },
                    new object[] { UcasResultType.PathwayResult, "Unclassified", "U" },
                    new object[] { UcasResultType.PathwayResult, "", "" },

                    // Specialism Result
                    new object[] { UcasResultType.SpecialismResult, "Distinction", "D" },
                    new object[] { UcasResultType.SpecialismResult, "Merit", "M" },
                    new object[] { UcasResultType.SpecialismResult, "Pass", "P" },
                    new object[] { UcasResultType.SpecialismResult, "Unclassified", "U" },
                    new object[] { UcasResultType.SpecialismResult, "", "" },

                    // Overall Result
                    new object[] { UcasResultType.OverallResult, "Distinction*", "D*" },
                    new object[] { UcasResultType.OverallResult, "Distinction", "D" },
                    new object[] { UcasResultType.OverallResult, "Merit", "M" },
                    new object[] { UcasResultType.OverallResult, "Pass", "P" },
                    new object[] { UcasResultType.OverallResult, "Partial achievement", "PA" },
                    new object[] { UcasResultType.OverallResult, "Unclassified", "U" },
                    new object[] { UcasResultType.OverallResult, "X - no result", "X" },
                };
            }
        }
    }
}
