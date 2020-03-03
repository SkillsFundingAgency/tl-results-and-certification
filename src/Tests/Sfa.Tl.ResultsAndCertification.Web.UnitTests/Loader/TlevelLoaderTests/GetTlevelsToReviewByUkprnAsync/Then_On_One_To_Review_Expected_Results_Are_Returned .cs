using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelsToReviewByUkprnAsync
{
    public class Then_On_One_To_Review_Expected_Results_Are_Returned : When_GetTlevelsToReviewByUkprnAsync__Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { Id = 1, PathwayId = 11, PathwayName = "Path11", RouteName = "Route1", StatusId = 1 },
            };

            InternalApiClient.GetAllTlevelsByUkprnAsync(Ukprn)
                .Returns(ApiClientResponse);

            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_One_Tlevels_AwaitingReview_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.TlevelsToReview.Should().NotBeNull();
            ActualResult.TlevelsToReview.Count().Should().Be(1);
        }

        [Fact]
        public void Then_IsOnlyOneTlevelReviewPending_Is_False()
        {
            ActualResult.IsOnlyOneTlevelReviewPending.Should().BeTrue();
        }

        [Fact]
        public void Then_ShowViewReviewedTlevelsLink_Is_True()
        {
            ActualResult.ShowViewReviewedTlevelsLink.Should().BeFalse();   
        }
    }
}
