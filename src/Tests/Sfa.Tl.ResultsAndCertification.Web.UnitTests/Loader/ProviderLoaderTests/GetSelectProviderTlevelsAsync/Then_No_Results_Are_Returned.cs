using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetSelectProviderTlevelsAsync
{
    public class Then_No_Results_Are_Returned : When_GetSelectProviderTlevelsAsync_Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = new ProviderTlevels
            {
                ProviderId = 1,
                DisplayName = "Test1",
                Ukprn = 12345
            };

            InternalApiClient.GetSelectProviderTlevelsAsync(Ukprn, ProviderId).Returns(ApiClientResponse);

            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_No_Provider_Tlevels_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProviderId.Should().Be(ApiClientResponse.ProviderId);
            ActualResult.DisplayName.Should().Be(ApiClientResponse.DisplayName);
            ActualResult.Ukprn.Should().Be(ApiClientResponse.Ukprn);
            ActualResult.Tlevels.Should().BeNullOrEmpty();
        }

    }
}
