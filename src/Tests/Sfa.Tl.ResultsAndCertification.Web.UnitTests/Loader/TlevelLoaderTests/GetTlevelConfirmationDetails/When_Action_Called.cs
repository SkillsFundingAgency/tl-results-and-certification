using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelConfirmationDetails
{
    public class When_Action_Called : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).GetAllTlevelsByUkprnAsync(Ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            
            ActualResult.PathwayId.Should().Be(PathwayId);
            ActualResult.ShowMoreTlevelsToReview.Should().Be(true);
            ActualResult.TlevelConfirmationText.Should().Be("T Level details confirmed");
            ActualResult.TlevelTitle.Should().Be("Tlevel Title1");
        }
    }
}
