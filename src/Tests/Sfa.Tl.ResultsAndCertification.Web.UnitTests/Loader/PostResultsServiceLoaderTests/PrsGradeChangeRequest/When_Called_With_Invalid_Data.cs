using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.PrsGradeChangeRequest
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private bool _expectedApiResult;

        public override void Given()
        {
            _expectedApiResult = false;
            InternalApiClient.PrsGradeChangeRequestAsync(Arg.Any<Models.Contracts.PostResultsService.PrsGradeChangeRequest>()).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_False_Returned()
        {
            ActualResult.Should().BeFalse();
        }
    }
}
