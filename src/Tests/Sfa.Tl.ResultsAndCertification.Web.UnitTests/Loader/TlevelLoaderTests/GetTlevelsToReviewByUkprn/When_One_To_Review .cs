using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelsToReviewByUkprn
{
    public class Then_On_One_To_Review_Expected_Results_Are_Returned : TestSetup
    {
        public override void Given()
        {
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { Id = 1, PathwayId = 11, TlevelTitle = "Tlevel Title", StatusId = 1 },
            };

            InternalApiClient.GetAllTlevelsByUkprnAsync(Ukprn)
                .Returns(ApiClientResponse);

            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.TlevelsToReview.Should().NotBeNull();
            ActualResult.TlevelsToReview.Count().Should().Be(1);
            ActualResult.IsOnlyOneTlevelReviewPending.Should().BeTrue();
            ActualResult.ShowViewReviewedTlevelsLink.Should().BeFalse();
        }
    }
}
