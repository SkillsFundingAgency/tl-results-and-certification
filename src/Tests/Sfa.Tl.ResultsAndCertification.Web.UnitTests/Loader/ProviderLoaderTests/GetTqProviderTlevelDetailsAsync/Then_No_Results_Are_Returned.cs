using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqProviderTlevelDetailsAsync
{
    public class Then_No_Results_Are_Returned : When_GetTqProviderTlevelDetailsAsync_Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = null;
            InternalApiClient.GetTqProviderTlevelDetailsAsync(Ukprn, TqProviderId).Returns(ApiClientResponse);

            Loader = new ProviderLoader(InternalApiClient, Mapper);
        }

        [Fact]
        public void Then_No_ProviderTlevelDetails_Are_Returned()
        {
            ActualResult.Should().BeNull();
        }
    }
}
