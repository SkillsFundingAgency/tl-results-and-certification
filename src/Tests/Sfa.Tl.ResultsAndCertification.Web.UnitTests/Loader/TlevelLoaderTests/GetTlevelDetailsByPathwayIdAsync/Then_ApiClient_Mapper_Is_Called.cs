using Xunit;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Models;
using FluentAssertions;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelDetailsByPathwayIdAsync
{
    public class Then_ApiClient_Mapper_Is_Called : When_Called_Method_GetTlevelDetailsByPathwayIdAsync
    {
        [Fact]
        public void Then_ApiClient_Is_Called()
        {
            InternalApiClient.Received().GetTlevelDetailsByPathwayIdAsync(Ukprn, Id);
        }

        [Fact]
        public void Then_Mapper_Is_Called()
        {
            Mapper.Received().Map<TLevelDetailsViewModel>(ApiClientResponse);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.PathwayId.Should().Be(ExpectedResult.PathwayId);
            ActualResult.RouteName.Should().Be(ExpectedResult.RouteName);
            ActualResult.PathwayName.Should().Be(ExpectedResult.PathwayName);
            ActualResult.ShowSomethingIsNotRight.Should().Be(ExpectedResult.ShowSomethingIsNotRight);
        }
    }
}
