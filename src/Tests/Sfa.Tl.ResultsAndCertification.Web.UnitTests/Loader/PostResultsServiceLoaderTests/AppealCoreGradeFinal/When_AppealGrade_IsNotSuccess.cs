using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.AppealCoreGradeFinal
{
    public class When_AppealGrade_IsNotSuccess : TestSetup
    {
        private bool expectedApiResult;

        public override void Given()
        {
            expectedApiResult = false;
            InternalApiClient.PrsActivityAsync(Arg.Any<PrsActivityRequest>()).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_False_Returned()
        {
            ActualResult.Should().BeFalse();
        }
    }
}