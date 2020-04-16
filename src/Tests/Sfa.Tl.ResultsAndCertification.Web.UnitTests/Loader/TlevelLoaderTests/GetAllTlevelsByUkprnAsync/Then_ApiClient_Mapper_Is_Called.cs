using Xunit;
using NSubstitute;
using FluentAssertions;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetAllTlevelsByUkprnAsync
{
    public class Then_ApiClient_Mapper_Is_Called : When_Called_Method_GetTlevelsByUkprnAsync
    {
        [Fact]
        public void Then_ApiClient_Is_Called()
        {
            InternalApiClient.Received().GetAllTlevelsByUkprnAsync(Ukprn);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsAnyReviewPending.Should().BeTrue();

            ActualResult.QueriedTlevels.Should().NotBeNull();
            ActualResult.QueriedTlevels.Count().Should().Be(1);
            ActualResult.QueriedTlevels.First().PathwayId.Should().Be(55);
            ActualResult.QueriedTlevels.First().TlevelTitle.Should().Be("P5");

            ActualResult.ConfirmedTlevels.Should().NotBeNull();
            ActualResult.ConfirmedTlevels.Count().Should().Be(3);
            ActualResult.ConfirmedTlevels.First().PathwayId.Should().Be(22);
            ActualResult.ConfirmedTlevels.First().TlevelTitle.Should().Be("P2");
        }
    }
}
