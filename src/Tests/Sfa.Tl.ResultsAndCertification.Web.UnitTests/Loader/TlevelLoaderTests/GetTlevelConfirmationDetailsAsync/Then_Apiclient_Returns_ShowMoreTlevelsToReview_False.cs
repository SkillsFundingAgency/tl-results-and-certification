using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelConfirmationDetailsAsync
{
    public class Then_Apiclient_Returns_ShowMoreTlevelsToReview_False : When_GetTlevelConfirmationDetailsAsync__Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { Id = 1, PathwayId = PathwayId, PathwayName = "Path11", RouteName = "Route1", StatusId = 2 },
            };

            InternalApiClient.GetAllTlevelsByUkprnAsync(Ukprn)
                .Returns(ApiClientResponse);

            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ShowMoreTlevelsToReview.Should().Be(false);

            ActualResult.PathwayId.Should().Be(PathwayId);
            ActualResult.TlevelConfirmationText.Should().Be("T Level details confirmed");
            ActualResult.TlevelTitle.Should().Be("Route1: Path11");
        }
    }
}
