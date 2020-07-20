using FluentAssertions;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegisteredProviderCoreDetailsAsync
{
    public class Then_Expected_Results_Are_Returned : When_GetRegisteredProviderCoreDetailsAsync_Is_Called
    {
        [Fact]
        public void Then_Two_Core_Details_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.CoreSelectList.Should().NotBeNull();
            ActualResult.CoreSelectList.Count().Should().Be(2);
        }

        [Fact]
        public void Then_Expected_Cores_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.CoreSelectList.Should().NotBeNullOrEmpty();

            ActualResult.CoreSelectList.Count.Should().Be(ApiClientResponse.Count);

            var expectedCoreResult = ApiClientResponse.FirstOrDefault();
            var actualCoreDetailsResult = ActualResult.CoreSelectList.FirstOrDefault();
            actualCoreDetailsResult.Should().NotBeNull();

            actualCoreDetailsResult.Value.Should().Be(expectedCoreResult.Code.ToString());
            actualCoreDetailsResult.Text.Should().Be($"{expectedCoreResult.Name} ({expectedCoreResult.Code})");
        }
    }
}
