using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelConfirmationDetailsAsync
{
    public class Then_Apiclient_Returns_ShowMoreTlevelsToReview_True : When_GetTlevelConfirmationDetailsAsync__Is_Called
    {
        [Fact]
        public void Then_ApiClient_Is_Called()
        {
            InternalApiClient.Received(1).GetAllTlevelsByUkprnAsync(Ukprn);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            
            ActualResult.PathwayId.Should().Be(PathwayId);
            ActualResult.ShowMoreTlevelsToReview.Should().Be(true);
            ActualResult.TlevelConfirmationText.Should().Be("T Level details confirmed");
            ActualResult.TlevelTitle.Should().Be("Route1: Path11");
        }
    }
}
