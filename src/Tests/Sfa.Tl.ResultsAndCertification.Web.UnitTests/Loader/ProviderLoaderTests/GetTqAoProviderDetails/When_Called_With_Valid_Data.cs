using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.GetTqAoProviderDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Count().Should().Be(2);

            var expectedProviderDetailsResult = ApiClientResponse.FirstOrDefault();
            var actualProviderDetailsResult = ActualResult.FirstOrDefault();
            actualProviderDetailsResult.Should().NotBeNull();

            actualProviderDetailsResult.ProviderId.Should().Be(expectedProviderDetailsResult.Id);
            actualProviderDetailsResult.DisplayName.Should().Be(expectedProviderDetailsResult.DisplayName);
            actualProviderDetailsResult.Ukprn.Should().Be(expectedProviderDetailsResult.Ukprn);
        }
    }
}
