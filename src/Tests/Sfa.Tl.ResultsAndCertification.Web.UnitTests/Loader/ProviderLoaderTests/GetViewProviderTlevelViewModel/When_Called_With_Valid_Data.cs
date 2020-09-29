using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetViewProviderTlevelViewModel
{
    public class When_Called_With_Valid_Data : TestSetup
    {        
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProviderId.Should().Be(ApiClientResponse.Id);
            ActualResult.DisplayName.Should().Be(ApiClientResponse.DisplayName);
            ActualResult.Ukprn.Should().Be(ApiClientResponse.Ukprn);
            ActualResult.Tlevels.Should().NotBeNull();
            ActualResult.Tlevels.Count().Should().Be(ApiClientResponse.Tlevels.Count());

            var expectedTlevelResult = ApiClientResponse.Tlevels.FirstOrDefault();
            var actualProviderTlevelResult = ActualResult.Tlevels.FirstOrDefault();
            actualProviderTlevelResult.Should().NotBeNull();

            actualProviderTlevelResult.TqProviderId.Should().Be(expectedTlevelResult.TqProviderId);
            actualProviderTlevelResult.TlevelTitle.Should().Be(expectedTlevelResult.TlevelTitle);
        }
    }
}
