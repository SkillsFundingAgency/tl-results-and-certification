using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.GetPrsLearnerDetailsTests
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private Models.Contracts.PostResultsService.PrsLearnerDetails expectedApiResult;

        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.GetPrsLearnerDetailsAsync(Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
