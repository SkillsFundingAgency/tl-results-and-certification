using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ProviderAddressLoaderTests.GetAddress
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private Models.Contracts.ProviderAddress.Address _expectedApiResult;

        public override void Given()
        {
            _expectedApiResult = null;
            InternalApiClient.GetAddressAsync(Arg.Any<long>()).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetAddressAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
