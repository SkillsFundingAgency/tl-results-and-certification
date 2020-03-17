using Xunit;
using NSubstitute;
using FluentAssertions;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelsByStatusIdAsync
{
    public class Then_Returns_Expected_Results : When_Called_Method_GetTlevelsByStatusIdAsync
    {
        [Fact]
        public void Then_ApiClient_Is_Called()
        {
            InternalApiClient.Received().GetTlevelsByStatusIdAsync(Ukprn, statusId);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();

            var actualResult = ActualResult.First();

            actualResult.PathwayId.Should().Be(ExpectedResult.PathwayId);
            actualResult.TlevelTitle.Should().Be(ExpectedTLevelTitle);
        }
    }
}
