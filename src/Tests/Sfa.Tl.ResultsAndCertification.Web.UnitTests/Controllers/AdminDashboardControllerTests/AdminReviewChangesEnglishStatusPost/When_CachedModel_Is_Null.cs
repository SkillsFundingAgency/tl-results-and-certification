using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesEnglishStatusPost
{
    public class When_CachedModel_Is_Null : TestSetup
    {
        public override void Given()
        {
            ViewModel = CreateViewModel(SubjectStatus.Achieved);

            CacheService.GetAsync<AdminChangeEnglishResultsViewModel>(CacheKey).Returns((AdminChangeEnglishResultsViewModel)null);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CacheService.Received(1).GetAsync<AdminChangeEnglishResultsViewModel>(CacheKey);

            AdminDashboardLoader.DidNotReceive().ProcessChangeEnglishStatusAsync(Arg.Any<AdminReviewChangesEnglishSubjectViewModel>());

            CacheService.DidNotReceive().SetAsync(
                Arg.Any<string>(),
                Arg.Any<NotificationBannerModel>(),
                Arg.Any<CacheExpiryTime>()
            );
            CacheService.DidNotReceive().SetAsync(
                Arg.Any<string>(),
                Arg.Any<AdminReviewChangesEnglishSubjectViewModel>()
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