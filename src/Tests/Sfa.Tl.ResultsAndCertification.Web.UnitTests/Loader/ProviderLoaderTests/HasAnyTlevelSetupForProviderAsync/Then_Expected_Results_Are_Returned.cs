using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.HasAnyTlevelSetupForProviderAsync
{
    public class Then_Expected_Results_Are_Returned : When_HasAnyTlevelSetupForProviderAsync_Is_Called
    {
        private bool expectedaApiResult = true;

        public override void Given()
        {
            InternalApiClient.HasAnyTlevelSetupForProviderAsync(Ukprn, TlProviderId)
                .Returns(expectedaApiResult);
        }

        [Fact]
        public void Then_HasAnyTlevelSetupForProviderAsync_Is_Called()
        {
            InternalApiClient.Received(1).HasAnyTlevelSetupForProviderAsync(Ukprn, TlProviderId);
        }

        [Fact]
        public void Then_Expected_LookupData_Are_Returned()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
