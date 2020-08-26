using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegisteredProviderCoreDetailsAsync
{
    public class Then_No_Results_Are_Returned : When_GetRegisteredProviderCoreDetailsAsync_Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = null;
            InternalApiClient.GetRegisteredProviderPathwayDetailsAsync(Ukprn, ProviderUkprn).Returns(ApiClientResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_No_SelectCoreViewModel_Are_Returned()
        {
            ActualResult.Should().BeNull();
        }
    }
}
