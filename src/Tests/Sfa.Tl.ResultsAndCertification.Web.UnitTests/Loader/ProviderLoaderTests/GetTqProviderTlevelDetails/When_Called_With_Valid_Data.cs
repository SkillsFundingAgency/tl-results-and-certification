using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqProviderTlevelDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.Id.Should().Be(ApiClientResponse.Id);
            ActualResult.DisplayName.Should().Be(ApiClientResponse.DisplayName);
            ActualResult.Ukprn.Should().Be(ApiClientResponse.Ukprn);

            ActualResult.TlevelTitle.Should().Be(ApiClientResponse.ProviderTlevel.TlevelTitle);
        }
    }
}
