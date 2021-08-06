using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradePost
{
    public class When_AppealGrade_IsNotSuccess : TestSetup
    {
        private AppealCoreGradeViewModel _mockLoderResponse;
        private readonly bool _appealGradeResponse = false;

        public override void Given()
        {
            ViewModel = new AppealCoreGradeViewModel { ProfileId = 1, PathwayAssessmentId = 11, AppealGrade = true };

            _mockLoderResponse = new AppealCoreGradeViewModel();
            Loader.GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId)
                .Returns(_mockLoderResponse);

            Loader.AppealCoreGradeAsync(AoUkprn, ViewModel).Returns(_appealGradeResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId);
            Loader.Received(1).AppealCoreGradeAsync(AoUkprn, ViewModel);
            CacheService.DidNotReceive().SetAsync(Arg.Any<string>(), Arg.Any<NotificationBannerModel>(), Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
