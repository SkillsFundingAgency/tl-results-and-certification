using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderLoaderTests.IsAnyProviderSetupCompleted
{
    public class When_Action_Called : TestSetup
    {
        private bool expectedaApiResult = true;

        public override void Given() 
        {
            InternalApiClient.IsAnyProviderSetupCompletedAsync(Ukprn)
                .Returns(expectedaApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).IsAnyProviderSetupCompletedAsync(Ukprn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
