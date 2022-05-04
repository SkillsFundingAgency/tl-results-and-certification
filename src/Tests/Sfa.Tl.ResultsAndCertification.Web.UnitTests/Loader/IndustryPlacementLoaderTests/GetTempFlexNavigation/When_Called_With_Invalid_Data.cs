using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.GetTempFlexNavigation
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given() { }
        
        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().BeNull();
        }
    }
}
