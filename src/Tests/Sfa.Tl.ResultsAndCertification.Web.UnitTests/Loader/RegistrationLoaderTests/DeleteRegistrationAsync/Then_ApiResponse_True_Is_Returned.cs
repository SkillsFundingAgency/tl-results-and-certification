using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.DeleteRegistrationAsync
{
    public class Then_ApiResponse_True_Is_Returned : When_DeleteRegistrationAsync_Is_Called
    {
        [Fact]
        public void Then_ApiClient_Is_Called()
        {
            InternalApiClient.Received().DeleteRegistrationAsync(Ukprn, ProfileId);
        }

        [Fact]
        public void Then_ApiResponse_Is_True()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
