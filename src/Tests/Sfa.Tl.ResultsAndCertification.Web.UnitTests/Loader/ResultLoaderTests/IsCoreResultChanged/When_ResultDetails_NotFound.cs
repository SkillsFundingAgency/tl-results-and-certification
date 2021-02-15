using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.IsCoreResultChanged
{
    public class When_ResultDetails_NotFound : TestSetup
    {
        public override void Given()
        {
            expectedApiResultDetails = null;
            InternalApiClient.GetResultDetailsAsync(AoUkprn, Arg.Any<int>(), RegistrationPathwayStatus.Active).Returns(expectedApiResultDetails);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }
    }
}
