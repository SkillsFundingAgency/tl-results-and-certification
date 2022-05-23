using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetTempFlexNavigation
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private IpTempFlexNavigation _mockIpTempFlexNavigation;
        public override void Given()
        {
            _mockIpTempFlexNavigation = new IpTempFlexNavigation { AskBlendedPlacement = true, AskTempFlexibility = true };
            InternalApiClient.GetTempFlexNavigationAsync(PathwayId, AcademicYear).Returns(_mockIpTempFlexNavigation);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.AskTempFlexibility.Should().Be(_mockIpTempFlexNavigation.AskBlendedPlacement);
            ActualResult.AskBlendedPlacement.Should().Be(_mockIpTempFlexNavigation.AskBlendedPlacement);
        }
    }
}
