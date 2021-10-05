using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.PrsGradeChangeRequest
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private readonly bool _expectedApiResult = true;
        public override void Given()
        {
            ViewModel = new PrsGradeChangeRequestViewModel
            {
                ProfileId = 1,
                AssessmentId = 2,
                ResultId = 3,
                ChangeRequestData = "Test"
            };

            InternalApiClient.PrsGradeChangeRequestAsync(Arg.Is<Models.Contracts.PostResultsService.PrsGradeChangeRequest>(x =>
                                x.ProfileId == ViewModel.ProfileId &&
                                x.AssessmentId == ViewModel.AssessmentId &&
                                x.ResultId == ViewModel.ResultId &&
                                x.RequestedMessage == ViewModel.ChangeRequestData &&
                                x.RequestedUserEmailAddress == Email)).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            InternalApiClient.Received(1).PrsGradeChangeRequestAsync(Arg.Is<Models.Contracts.PostResultsService.PrsGradeChangeRequest>(x =>
                                x.ProfileId == ViewModel.ProfileId &&
                                x.AssessmentId == ViewModel.AssessmentId &&
                                x.ResultId == ViewModel.ResultId &&
                                x.RequestedMessage == ViewModel.ChangeRequestData &&
                                x.RequestedUserEmailAddress == Email));
        }

        [Fact]
        public void Then_True_Returned()
        {
            ActualResult.Should().BeTrue();
        }
    }
}
