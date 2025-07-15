using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesEnglishStatusPost
{
    public class When_CachedModel_Is_Null : TestSetup
    {
        public override void Given()
        {
            ViewModel = CreateViewModel(SubjectStatus.Achieved);

            CacheService.GetAsync<AdminChangeEnglishStatusViewModel>(CacheKey).Returns((AdminChangeEnglishStatusViewModel)null);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CacheService.Received(1).GetAsync<AdminChangeEnglishStatusViewModel>(CacheKey);

            AdminDashboardLoader.DidNotReceive().ProcessChangeEnglishStatusAsync(Arg.Any<AdminReviewChangesEnglishStatusViewModel>());

            CacheService.DidNotReceive().SetAsync(
                Arg.Any<string>(),
                Arg.Any<NotificationBannerModel>(),
                Arg.Any<CacheExpiryTime>()
            );
            CacheService.DidNotReceive().SetAsync(
                Arg.Any<string>(),
                Arg.Any<AdminReviewChangesEnglishStatusViewModel>()
            );
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var redirectResult = ActualResult.Should().BeOfType<RedirectToRouteResult>().Subject;

            redirectResult.RouteName.Should().Be(RouteConstants.PageNotFound);
            redirectResult.RouteValues.Should().BeNull();
        }
    }
}