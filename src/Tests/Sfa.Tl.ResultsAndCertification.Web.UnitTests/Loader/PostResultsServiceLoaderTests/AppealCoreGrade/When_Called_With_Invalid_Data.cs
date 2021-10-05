using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.AppealCoreGrade
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private bool expectedApiResult;

        public override void Given()
        {
            expectedApiResult = false;
            InternalApiClient.AppealGradeAsync(Arg.Any<AppealGradeRequest>()).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_False_Returned()
        {
            ActualResult.Should().BeFalse();
        }
    }
}