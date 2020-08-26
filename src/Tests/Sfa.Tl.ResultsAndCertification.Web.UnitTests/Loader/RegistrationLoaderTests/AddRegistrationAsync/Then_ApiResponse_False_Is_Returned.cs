using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.AddRegistrationAsync
{
    public class Then_ApiResponse_False_Is_Returned : When_AddRegistrationAsync_Is_Called
    {
        public override void Given()
        {
            ApiClientResponse = false;
            InternalApiClient.AddRegistrationAsync(Arg.Any<RegistrationRequest>()).Returns(ApiClientResponse);
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_ApiResponse_Is_False()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
