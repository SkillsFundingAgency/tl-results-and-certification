using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqAoProviderDetailsAsync
{
    public class Then_Expected_Results_Are_Returned : When_GetTqAoProviderDetailsAsync_Is_Called
    {
        [Fact]
        public void Then_Two_Provider_Details_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Count().Should().Be(2);
        }

        [Fact]
        public void Then_Expected_Provider_Details_Are_Returned()
        {
            ActualResult.Should().NotBeNull();

            var expectedProviderDetailsResult = ApiClientResponse.FirstOrDefault();
            var actualProviderDetailsResult = ActualResult.FirstOrDefault();
            actualProviderDetailsResult.Should().NotBeNull();

            actualProviderDetailsResult.ProviderId.Should().Be(expectedProviderDetailsResult.Id);
            actualProviderDetailsResult.DisplayName.Should().Be(expectedProviderDetailsResult.DisplayName);
            actualProviderDetailsResult.Ukprn.Should().Be(expectedProviderDetailsResult.Ukprn);
        }
    }
}
