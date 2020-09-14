using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.RemoveTqProviderTlevel
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            ApiClientResponse = false;
            InternalApiClient.RemoveTqProviderTlevelAsync(Ukprn, TqProviderId).Returns(ApiClientResponse);
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
