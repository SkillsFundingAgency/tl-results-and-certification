using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers.CommonHelperTests
{
    public class When_IsValidGradeForChangeResult_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Object params { ComponentType, ComponentType, ExpectedResult }
                    new object[] { "PCG1", ComponentType.Core, false },
                    new object[] { "PCG2", ComponentType.Core, false },
                    new object[] { "PCG8", ComponentType.Core, true },
                    new object[] { "SCG1", ComponentType.Specialism, false },
                    new object[] { "SCG2", ComponentType.Specialism, false },
                    new object[] { "SCG5", ComponentType.Specialism, true }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(string gradeCode, ComponentType componentType, bool expectedResult)
        {
            var actualResult = CommonHelper.IsValidGradeForChangeResult(gradeCode, componentType);
            actualResult.Should().Be(expectedResult);
        }
    }
}
