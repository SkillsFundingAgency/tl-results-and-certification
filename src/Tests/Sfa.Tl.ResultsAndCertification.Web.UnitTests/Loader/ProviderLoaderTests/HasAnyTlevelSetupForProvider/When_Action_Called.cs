using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.HasAnyTlevelSetupForProvider
{
    public class When_Action_Called : TestSetup
    {
        private bool expectedaApiResult = true;

        public override void Given()
        {
            InternalApiClient.HasAnyTlevelSetupForProviderAsync(Ukprn, TlProviderId).Returns(expectedaApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).HasAnyTlevelSetupForProviderAsync(Ukprn, TlProviderId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
