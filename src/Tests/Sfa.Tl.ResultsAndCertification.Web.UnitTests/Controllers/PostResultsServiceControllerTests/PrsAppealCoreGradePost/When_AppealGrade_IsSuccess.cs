using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using Xunit;
using AppealCoreGradeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.AppealCoreGrade;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealCoreGradePost
{
    public class When_AppealGrade_IsSuccess : TestSetup
    {
        private AppealCoreGradeViewModel _mockLoderResponse;
        private readonly bool _appealGradeResponse = true;
        private string _expectedSuccessBannerMsg;

        public override void Given()
        {
            ViewModel = new AppealCoreGradeViewModel { ProfileId = 1, PathwayAssessmentId = 11, PathwayName = "Education", PathwayCode = "9856231479", AppealGrade = true };

            _mockLoderResponse = new AppealCoreGradeViewModel { PathwayName = ViewModel.PathwayName, PathwayCode = ViewModel.PathwayCode } ;
            Loader.GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId)
                .Returns(_mockLoderResponse);

            Loader.AppealCoreGradeAsync(AoUkprn, ViewModel).Returns(_appealGradeResponse);
            _expectedSuccessBannerMsg = string.Format(AppealCoreGradeContent.Banner_Message, $"{ViewModel.PathwayName} ({ViewModel.PathwayCode})");
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<AppealCoreGradeViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.PathwayAssessmentId);
            Loader.Received(1).AppealCoreGradeAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
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
