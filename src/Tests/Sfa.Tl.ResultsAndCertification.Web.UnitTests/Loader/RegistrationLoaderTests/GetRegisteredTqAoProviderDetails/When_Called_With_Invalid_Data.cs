using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegisteredTqAoProviderDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            ApiClientResponse = null;
            InternalApiClient.GetTqAoProviderDetailsAsync(Ukprn).Returns(ApiClientResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
