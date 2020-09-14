using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqProviderTlevelDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            ApiClientResponse = null;
            InternalApiClient.GetTqProviderTlevelDetailsAsync(Ukprn, TqProviderId).Returns(ApiClientResponse);
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
