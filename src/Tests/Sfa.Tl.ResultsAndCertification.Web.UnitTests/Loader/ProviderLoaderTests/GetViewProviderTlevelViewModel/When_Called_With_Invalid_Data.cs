using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetViewProviderTlevelViewModel
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            ApiClientResponse = new ProviderTlevels
            {
                Id = 1,
                DisplayName = "Test1",
                Ukprn = 12345
            };

            InternalApiClient.GetAllProviderTlevelsAsync(Ukprn, ProviderId).Returns(ApiClientResponse);
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProviderId.Should().Be(ApiClientResponse.Id);
            ActualResult.DisplayName.Should().Be(ApiClientResponse.DisplayName);
            ActualResult.Ukprn.Should().Be(ApiClientResponse.Ukprn);
            ActualResult.Tlevels.Should().BeNullOrEmpty();
        }
    }
}
