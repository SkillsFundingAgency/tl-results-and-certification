using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultWithdrawnViewModel
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            InternalApiClient.Received(1).GetResultDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Withdrawn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
