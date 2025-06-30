using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesEnglishStatusPost
{
    public class When_Success : TestSetup
    {
        private AdminChangeEnglishResultsViewModel _cacheModel;

        public override void Given()
        {
            ViewModel = CreateViewModel(SubjectStatus.Achieved);

            _cacheModel = new AdminChangeEnglishResultsViewModel
            {
                RegistrationPathwayId = 1,
                EnglishStatusTo = SubjectStatus.Achieved
            };

            CacheService.GetAsync<AdminChangeEnglishResultsViewModel>(CacheKey).Returns(_cacheModel);
            AdminDashboardLoader.ProcessChangeEnglishStatusAsync(ViewModel).Returns(true);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            AdminDashboardLoader.Received(1).ProcessChangeEnglishStatusAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_Expected_ActionResult()
        {
            var route = ActualResult as RedirectToActionResult;
            route.ActionName.Should().Be(RouteConstants.AdminLearnerRecord);
            route.RouteValues[Constants.PathwayId].Should().Be(_cacheModel.RegistrationPathwayId);
        }

        [Fact]
        public void Then_CacheService_Is_Called_With_Success_Banner()
        {
            CacheService.Received(1)
                .SetAsync(
                    Arg.Is<string>(s => s.Contains(CacheConstants.AdminDashboardCacheKey)),
                    Arg.Is<NotificationBannerModel>(b =>
                        b.DisplayMessageBody == true &&
                        b.Message == ReviewChangeLevelTwoEnglish.Message_Notification_Success &&
                        b.IsRawHtml == true),
                    Arg.Any<CacheExpiryTime>());
        }
    }
}