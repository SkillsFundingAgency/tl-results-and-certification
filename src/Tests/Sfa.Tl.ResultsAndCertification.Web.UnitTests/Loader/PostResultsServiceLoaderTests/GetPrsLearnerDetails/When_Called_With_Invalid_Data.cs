using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.GetPrsLearnerDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private Models.Contracts.PostResultsService.PrsLearnerDetails expectedApiResult;
        protected PrsLearnerDetailsViewModel1 ActualResult { get; set; }

        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.GetPrsLearnerDetailsAsync(Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>()).Returns(expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel1>(AoUkprn, ProfileId, AssessmentId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
