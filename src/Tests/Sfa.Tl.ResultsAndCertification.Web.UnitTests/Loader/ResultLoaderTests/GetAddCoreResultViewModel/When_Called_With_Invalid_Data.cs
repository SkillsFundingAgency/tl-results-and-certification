using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetAddCoreResultViewModel
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResultDetails = null;
            InternalApiClient.GetResultDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResultDetails);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }
    }
}
