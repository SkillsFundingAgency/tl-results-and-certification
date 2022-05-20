using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.GetPrsLearnerDetails
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private LearnerRecord expectedApiResult;

        protected PrsLearnerDetailsViewModel ActualResult { get; set; }

        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.GetLearnerRecordAsync(Arg.Any<long>(), Arg.Any<int>(), Arg.Any<RegistrationPathwayStatus?>()).Returns(expectedApiResult);
        }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId, AssessmentId, ComponentType);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
