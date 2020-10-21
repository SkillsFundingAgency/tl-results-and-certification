using FluentAssertions;
using NSubstitute;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetAllTlevelsByUkprn
{
    public class When_Action_Called : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received().GetAllTlevelsByUkprnAsync(Ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
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
