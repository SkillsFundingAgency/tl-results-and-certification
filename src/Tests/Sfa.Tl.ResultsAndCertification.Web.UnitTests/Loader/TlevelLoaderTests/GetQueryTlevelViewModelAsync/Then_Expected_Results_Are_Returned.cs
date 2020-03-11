using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetQueryTlevelViewModelAsync
{
    public class Then_Expected_Results_Are_Returned : When_GetQueryTlevelViewModelAsync_Is_Called
    {
        [Fact]
        public void Then_Mapper_Returned_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.PathwayId.Should().Be(ApiClientResponse.PathwayId);
            ActualResult.PathwayName.Should().Be(ApiClientResponse.PathwayName);
            ActualResult.PathwayStatusId.Should().Be(ApiClientResponse.PathwayStatusId);
            ActualResult.TqAwardingOrganisationId.Should().Be(ApiClientResponse.TqAwardingOrganisationId);

            ActualResult.Specialisms.Should().NotBeNull();
            ActualResult.Specialisms.Count().Should().Be(2);
            ActualResult.Specialisms.First().Should().Be(ApiClientResponse.Specialisms.First());

            ActualResult.Query.Should().BeNull();
        }
    }
}
