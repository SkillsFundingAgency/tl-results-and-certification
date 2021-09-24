using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetPathwaySpecialismsByPathwayLarId
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.PathwayId.Should().Be(ApiClientResponse.Id);
            ActualResult.PathwayName.Should().Be(ApiClientResponse.PathwayName);
            ActualResult.PathwayCode.Should().Be(ApiClientResponse.PathwayCode);

            ActualResult.Specialisms.Should().NotBeNullOrEmpty();
            ActualResult.Specialisms.Count.Should().Be(2);

            ActualResult.Specialisms[0].Code.Should().Be("33333333");
            ActualResult.Specialisms[0].DisplayName.Should().Be("Arts (33333333)");

            ActualResult.Specialisms[1].Code.Should().Be("11111111|22222222");
            ActualResult.Specialisms[1].DisplayName.Should().Be("Design (11111111) and Engineering (22222222)");
        }
    }
}
