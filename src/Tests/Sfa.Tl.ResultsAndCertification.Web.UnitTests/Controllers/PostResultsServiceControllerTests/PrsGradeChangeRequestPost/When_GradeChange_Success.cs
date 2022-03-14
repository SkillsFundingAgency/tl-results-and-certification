using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsGradeChangeRequestPost
{
    public class When_GradeChange_Success : TestSetup
    {
        private readonly bool _gradeChangeRequestResponse = true;
        private PrsGradeChangeRequestViewModel _mockGradeChangeRequestViewModel;

        public override void Given()
        {
            ViewModel = new PrsGradeChangeRequestViewModel
            {
                ProfileId = 1,
                AssessmentId = 2,
                ResultId = 3,
                ChangeRequestData = "Change grade"
            };

            _mockGradeChangeRequestViewModel = new PrsGradeChangeRequestViewModel
            {
                ProfileId = ViewModel.ProfileId,
                AssessmentId = ViewModel.AssessmentId,
                ResultId = 10,
                Status = RegistrationPathwayStatus.Active,
                PrsStatus = PrsStatus.Final
            };

            Loader.GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType.Core).Returns(_mockGradeChangeRequestViewModel);
            Loader.PrsGradeChangeRequestAsync(ViewModel).Returns(_gradeChangeRequestResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsGradeChangeRequestViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType.Core);
            Loader.Received(1).PrsGradeChangeRequestAsync(ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<PrsGradeChangeRequestConfirmationViewModel>
                (x => x.ProfileId == ViewModel.ProfileId), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_PrsGradeChangeRequestConfirmation()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsGradeChangeRequestConfirmation);
        }
    }
}
