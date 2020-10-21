using Xunit;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using FluentAssertions;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelDetailsByPathwayId
{
    public class When_Action_Called : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            Mapper.Received().Map<TLevelDetailsViewModel>(ApiClientResponse);
            InternalApiClient.Received().GetTlevelDetailsByPathwayIdAsync(Ukprn, Id);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.PathwayId.Should().Be(ExpectedResult.PathwayId);
            ActualResult.RouteName.Should().Be(ExpectedResult.RouteName);
            ActualResult.PathwayName.Should().Be(ExpectedResult.PathwayName);
            ActualResult.ShowSomethingIsNotRight.Should().Be(ExpectedResult.ShowSomethingIsNotRight);
            ActualResult.ShowQueriedInfo.Should().Be(ExpectedResult.ShowQueriedInfo);
        }
    }
}
