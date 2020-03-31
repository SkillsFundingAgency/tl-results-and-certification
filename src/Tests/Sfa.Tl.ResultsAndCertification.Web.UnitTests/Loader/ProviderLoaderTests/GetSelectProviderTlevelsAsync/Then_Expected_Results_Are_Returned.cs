using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetSelectProviderTlevelsAsync
{
    public class Then_Expected_Results_Are_Returned : When_GetSelectProviderTlevelsAsync_Is_Called
    {
        [Fact]
        public void Then_Two_Provider_Tlevels_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Tlevels.Should().NotBeNull();
            ActualResult.Tlevels.Count().Should().Be(2);
        }

        [Fact]
        public void Then_Expected_Provider_Tlevels_Are_Returned()
        {            
            ActualResult.Should().NotBeNull();
            ActualResult.ProviderId.Should().Be(ApiClientResponse.ProviderId);
            ActualResult.DisplayName.Should().Be(ApiClientResponse.DisplayName);
            ActualResult.Ukprn.Should().Be(ApiClientResponse.Ukprn);
            ActualResult.Tlevels.Should().NotBeNull();

            var expectedTlevelResult = ApiClientResponse.Tlevels.FirstOrDefault();
            var actualProviderTlevelResult = ActualResult.Tlevels.FirstOrDefault();
            actualProviderTlevelResult.Should().NotBeNull();

            actualProviderTlevelResult.TqAwardingOrganisationId.Should().Be(expectedTlevelResult.TqAwardingOrganisationId);
            actualProviderTlevelResult.PathwayId.Should().Be(expectedTlevelResult.PathwayId);
            actualProviderTlevelResult.TlevelTitle.Should().Be($"{expectedTlevelResult.RouteName}: {expectedTlevelResult.PathwayName}");
        }
    }
}
