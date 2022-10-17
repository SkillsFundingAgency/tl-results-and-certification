using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Application.Helpers;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Helpers.DocumentPrintHelperTests
{
    public class When_FormatLearnerName_Is_Called
    {
        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] {"John", "Smith", "John Smith"},
                    new object[] {"JOHN", "Smith", "John Smith"},
                    new object[] {"McDonald", "Forest", "McDonald Forest"},
                    new object[] {"john", "smith", "John Smith"}
                };
            }
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Expected_Results_Are_Returned(string fname, string lname, string expectedResult)
        {
            var actualResult = DocumentPrintHelper.FormatLearnerName(fname, lname);
            actualResult.Should().Be(expectedResult);
        }
    }
}
