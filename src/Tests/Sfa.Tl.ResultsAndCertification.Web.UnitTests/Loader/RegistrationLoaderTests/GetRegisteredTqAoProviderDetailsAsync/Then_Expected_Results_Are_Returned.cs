using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegisteredTqAoProviderDetailsAsync
{
    public class Then_Expected_Results_Are_Returned : When_GetRegisteredTqAoProviderDetailsAsync_Is_Called
    {
        [Fact]
        public void Then_Two_Provider_Details_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProvidersSelectList.Should().NotBeNull();
            ActualResult.ProvidersSelectList.Count().Should().Be(2);
        }

        [Fact]
        public void Then_Expected_Providers_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProvidersSelectList.Should().NotBeNullOrEmpty();

            ActualResult.ProvidersSelectList.Count.Should().Be(ApiClientResponse.Count);

            var expectedProviderResult = ApiClientResponse.FirstOrDefault();
            var actualProviderDetailsResult = ActualResult.ProvidersSelectList.FirstOrDefault();
            actualProviderDetailsResult.Should().NotBeNull();

            actualProviderDetailsResult.Value.Should().Be(expectedProviderResult.Id.ToString());
            actualProviderDetailsResult.Text.Should().Be($"{expectedProviderResult.DisplayName} ({expectedProviderResult.Ukprn})" );
        }
    }
}
