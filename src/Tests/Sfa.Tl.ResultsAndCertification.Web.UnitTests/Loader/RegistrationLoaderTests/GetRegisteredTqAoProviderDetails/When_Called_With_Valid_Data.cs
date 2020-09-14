using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegisteredTqAoProviderDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProvidersSelectList.Should().NotBeNullOrEmpty();
            ActualResult.ProvidersSelectList.Count.Should().Be(ApiClientResponse.Count);

            var expectedProviderResult = ApiClientResponse.FirstOrDefault();
            var actualProviderDetailsResult = ActualResult.ProvidersSelectList.FirstOrDefault();
            actualProviderDetailsResult.Should().NotBeNull();

            actualProviderDetailsResult.Value.Should().Be(expectedProviderResult.Ukprn.ToString());
            actualProviderDetailsResult.Text.Should().Be($"{expectedProviderResult.DisplayName} ({expectedProviderResult.Ukprn})" );
        }
    }
}
