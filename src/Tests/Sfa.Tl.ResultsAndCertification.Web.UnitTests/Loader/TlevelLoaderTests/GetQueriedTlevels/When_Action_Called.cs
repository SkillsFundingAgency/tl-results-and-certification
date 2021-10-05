using Xunit;
using NSubstitute;
using FluentAssertions;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetQueriedTlevels
{
    public class When_Action_Called : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).GetTlevelsByStatusIdAsync(Ukprn, (int)TlevelReviewStatus.Queried);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.Tlevels.Should().NotBeNull();
            ActualResult.Tlevels.Count.Should().Be(1);
            ActualResult.Tlevels.First().PathwayId.Should().Be(ExpectedResult.PathwayId);
            ActualResult.Tlevels.First().TlevelTitle.Should().Be(ExpectedResult.TlevelTitle);
        }
    }
}
