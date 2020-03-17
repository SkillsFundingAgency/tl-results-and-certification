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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TlevelLoaderTests.GetTlevelConfirmationDetailsAsync
{
    public class Then_Apiclient_Returns_IsQueried_true : When_GetTlevelConfirmationDetailsAsync__Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = new List<AwardingOrganisationPathwayStatus>
            {
                new AwardingOrganisationPathwayStatus { Id = 1, PathwayId = PathwayId2, PathwayName = "Path11", RouteName = "Route1", StatusId = 3 },
            };

            InternalApiClient.GetAllTlevelsByUkprnAsync(Ukprn).Returns(ApiClientResponse);

            PathwayId = ApiClientResponse.FirstOrDefault().PathwayId;
            Loader = new TlevelLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            var expectedReponse = ApiClientResponse.FirstOrDefault();

            ;
            var expectedConfirmationText = string.Format(Confirmation.Section_Heading, EnumExtensions.GetEnumValue<TlevelReviewStatus>(expectedReponse.StatusId).ToString().ToLowerInvariant());
            var expectedTlevelTitle = $"{expectedReponse.RouteName}: {expectedReponse.PathwayName}";

            ActualResult.Should().NotBeNull();
            ActualResult.IsQueried.Should().Be(true);

            ActualResult.PathwayId.Should().Be(PathwayId);
            ActualResult.TlevelConfirmationText.Should().Be(expectedConfirmationText);
            ActualResult.TlevelTitle.Should().Be(expectedTlevelTitle);
        }
    }
}
