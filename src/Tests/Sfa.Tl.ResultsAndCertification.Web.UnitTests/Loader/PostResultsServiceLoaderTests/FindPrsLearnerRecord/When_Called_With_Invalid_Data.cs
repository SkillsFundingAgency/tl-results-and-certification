using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.FindPrsLearnerRecord
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private Models.Contracts.PostResultsService.FindPrsLearnerRecord expectedApiResult;

        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.FindPrsLearnerRecordAsync(Arg.Any<long>(), Arg.Any<long>()).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
