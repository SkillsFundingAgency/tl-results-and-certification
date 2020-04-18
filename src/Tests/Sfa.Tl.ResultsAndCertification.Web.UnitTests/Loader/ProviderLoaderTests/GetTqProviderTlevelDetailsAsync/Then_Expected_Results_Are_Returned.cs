using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqProviderTlevelDetailsAsync
{
    public class Then_Expected_Results_Are_Returned : When_GetTqProviderTlevelDetailsAsync_Is_Called
    {
        [Fact]
        public void Then_Expected_Provider_Tlevel_Details_Are_Returned()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.Id.Should().Be(ApiClientResponse.Id);
            ActualResult.DisplayName.Should().Be(ApiClientResponse.DisplayName);
            ActualResult.Ukprn.Should().Be(ApiClientResponse.Ukprn);

            ActualResult.TlevelTitle.Should().Be(ApiClientResponse.ProviderTlevel.TlevelTitle);
        }
    }
}
