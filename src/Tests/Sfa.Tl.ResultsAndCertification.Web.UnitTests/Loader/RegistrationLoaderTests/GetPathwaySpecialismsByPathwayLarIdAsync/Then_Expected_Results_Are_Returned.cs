using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetPathwaySpecialismsByPathwayLarIdAsync
{
    public class Then_Expected_Results_Are_Returned : When_GetPathwaySpecialismsByPathwayLarIdAsync_Is_Called
    {
        [Fact]
        public void Then_Expected_Specialisms_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Specialisms.Should().NotBeNullOrEmpty();

            ActualResult.Specialisms.Count.Should().Be(ApiClientResponse.Specialisms.Count);

            var expectedSpecialismResult = ApiClientResponse.Specialisms.FirstOrDefault();
            var actualSpecialismResult = ActualResult.Specialisms.FirstOrDefault();
            actualSpecialismResult.Should().NotBeNull();

            actualSpecialismResult.Id.Should().Be(expectedSpecialismResult.Id);
            actualSpecialismResult.Code.Should().Be(expectedSpecialismResult.Code);
            actualSpecialismResult.Name.Should().Be(expectedSpecialismResult.Name);
            actualSpecialismResult.DisplayName.Should().Be($"{expectedSpecialismResult.Name} ({expectedSpecialismResult.Code})");
        }
    }
}
