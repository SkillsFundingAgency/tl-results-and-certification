using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelConfirmationDetails
{
    public class When_Called_With_Confirmed_Status : TestSetup
    {
        public override void Given()
        {
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { Id = 1, PathwayId = PathwayId, TlevelTitle = "Tlevel Title", StatusId = 2 },
            };

            InternalApiClient.GetAllTlevelsByUkprnAsync(Ukprn).Returns(ApiClientResponse);
            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var expectedReponse = ApiClientResponse.FirstOrDefault();
            var expectedConfirmationText = string.Format(Confirmation.Section_Heading, EnumExtensions.GetEnumValue<TlevelReviewStatus>(expectedReponse.StatusId).ToString().ToLowerInvariant());
            var expectedTlevelTitle = expectedReponse.TlevelTitle;

            ActualResult.Should().NotBeNull();
            ActualResult.IsQueried.Should().Be(false);
            ActualResult.ShowMoreTlevelsToReview.Should().Be(false);

            ActualResult.PathwayId.Should().Be(PathwayId);
            ActualResult.TlevelConfirmationText.Should().Be(expectedConfirmationText);
            ActualResult.TlevelTitle.Should().Be(expectedTlevelTitle);
        }
    }
}
