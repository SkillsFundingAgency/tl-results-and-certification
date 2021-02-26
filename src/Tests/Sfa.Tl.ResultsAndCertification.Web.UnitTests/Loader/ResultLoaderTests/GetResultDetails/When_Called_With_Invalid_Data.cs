using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.GetResultDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
