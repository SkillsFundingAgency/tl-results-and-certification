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
    public class When_AppealGrade_IsSuccess : TestSetup
    {
        private AppealCoreGradeViewModel mockLoderResponse;
        private readonly bool appealGradeResponse = true;

        public override void Given()
        {
            ViewModel = new AppealCoreGradeViewModel { ProfileId = 1, PathwayAssessmentId = 11, AppealGrade = true };

            mockLoderResponse = new AppealCoreGradeViewModel();
            Loader.GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId)
                .Returns(mockLoderResponse);

            Loader.AppealCoreGradeAsync(AoUkprn, ViewModel).Returns(appealGradeResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId);
            Loader.Received(1).AppealCoreGradeAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<NotificationBannerModel>(), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(2);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
            route.RouteValues[Constants.AssessmentId].Should().Be(ViewModel.PathwayAssessmentId);
        }
    }
}
