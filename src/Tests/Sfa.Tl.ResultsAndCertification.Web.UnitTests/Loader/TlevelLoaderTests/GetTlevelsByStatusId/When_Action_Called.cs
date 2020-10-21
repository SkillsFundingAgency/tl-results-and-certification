using Xunit;
using NSubstitute;
using FluentAssertions;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelsByStatusId
{
    public class When_Action_Called : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).GetTlevelsByStatusIdAsync(Ukprn, statusId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            var actualResult = ActualResult.First();
            actualResult.PathwayId.Should().Be(ExpectedResult.PathwayId);
            actualResult.TlevelTitle.Should().Be(ExpectedTLevelTitle);
        }
    }
}
