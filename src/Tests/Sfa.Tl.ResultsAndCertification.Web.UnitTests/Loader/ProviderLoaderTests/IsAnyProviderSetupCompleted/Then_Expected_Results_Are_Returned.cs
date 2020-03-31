using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.IsAnyProviderSetupCompleted
{
    public class Then_Expected_Results_Are_Returned : When_IsAnyProviderSetupCompletedasync_Is_Called
    {
        private bool expectedaApiResult = true;

        public override void Given() 
        {
            InternalApiClient.IsAnyProviderSetupCompletedAsync(Ukprn)
                .Returns(expectedaApiResult);
        }

        [Fact]
        public void Then_IsAnyProviderSetupCompletedAsync_Is_Called()
        {
            InternalApiClient.Received(1).IsAnyProviderSetupCompletedAsync(Ukprn);
        }

        [Fact]
        public void Then_Expected_LookupData_Are_Returned()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
