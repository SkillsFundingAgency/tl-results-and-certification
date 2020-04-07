using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.RemoveTqProviderTlevelAsync
{
    public class Then_ApiResponse_False_Is_Returned : When_RemoveProviderTlevelAsync_Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = false;
            InternalApiClient.RemoveTqProviderTlevelAsync(Ukprn, TqProviderId).Returns(ApiClientResponse);
            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_ApiResponse_Is_False()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
