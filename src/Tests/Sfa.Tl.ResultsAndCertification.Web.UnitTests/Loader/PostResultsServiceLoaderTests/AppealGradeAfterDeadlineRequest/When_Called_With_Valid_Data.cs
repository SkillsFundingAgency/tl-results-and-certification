using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.AppealGradeAfterDeadlineRequest
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private readonly bool _expectedApiResult = true;
        public override void Given()
        {
            CreateMapper();
            ViewModel = new AppealGradeAfterDeadlineConfirmViewModel
            {
                ProfileId = 1,
                PathwayAssessmentId = 2,
                PathwayResultId = 3
            };

            InternalApiClient.AppealGradeAfterDeadlineRequestAsync(Arg.Is<Models.Contracts.PostResultsService.AppealGradeAfterDeadlineRequest>(x =>
                                x.ProfileId == ViewModel.ProfileId &&
                                x.AssessmentId == ViewModel.PathwayAssessmentId &&
                                x.ResultId == ViewModel.PathwayResultId &&
                                x.RequestedUserEmailAddress == Email)).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).AppealGradeAfterDeadlineRequestAsync(Arg.Is<Models.Contracts.PostResultsService.AppealGradeAfterDeadlineRequest>(x =>
                                x.ProfileId == ViewModel.ProfileId &&
                                x.AssessmentId == ViewModel.PathwayAssessmentId &&
                                x.ResultId == ViewModel.PathwayResultId &&
                                x.RequestedUserEmailAddress == Email));
        }

        [Fact]
        public void Then_True_Returned()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
