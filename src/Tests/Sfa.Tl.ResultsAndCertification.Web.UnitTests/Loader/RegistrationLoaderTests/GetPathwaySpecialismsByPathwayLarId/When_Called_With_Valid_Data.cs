using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetPathwaySpecialismsByPathwayLarId
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Specialisms.Should().NotBeNullOrEmpty();

            ActualResult.Specialisms.Count.Should().Be(ApiClientResponse.Specialisms.Count());

            var expectedSpecialismResult = ApiClientResponse.Specialisms.FirstOrDefault().SpecialismDetails.FirstOrDefault();
            var actualSpecialismResult = ActualResult.Specialisms.FirstOrDefault();
            actualSpecialismResult.Should().NotBeNull();

            actualSpecialismResult.Code.Should().Be(expectedSpecialismResult.Code);
            actualSpecialismResult.DisplayName.Should().Be($"{expectedSpecialismResult.Name} ({expectedSpecialismResult.Code})");
        }
    }
}
