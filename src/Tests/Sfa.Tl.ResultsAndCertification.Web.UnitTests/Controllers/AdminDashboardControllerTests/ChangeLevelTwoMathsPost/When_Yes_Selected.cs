using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeLevelTwoMathsPost
{
    public class When_Yes_Selected : TestSetup
    {
        private const int ExpectedRegistrationPathwayId = 1;
        private AdminChangeResultsViewModel _originalViewModel;

        public override void Given()
        {
            ViewModel = CreateViewModel(ExpectedRegistrationPathwayId, null, SubjectStatus.Achieved);

            // Set up the original view model that will be returned by the loader
            _originalViewModel = CreateViewModel(ExpectedRegistrationPathwayId, SubjectStatus.NotAchieved, null);

            AdminDashboardLoader
                .GetAdminLearnerRecordAsync<AdminChangeResultsViewModel>(ExpectedRegistrationPathwayId)
                .Returns(_originalViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            AdminDashboardLoader
                .Received(1)
                .GetAdminLearnerRecordAsync<AdminChangeResultsViewModel>(ExpectedRegistrationPathwayId);

            CacheService
                .Received(1)
                .SetAsync(Arg.Is<string>(s => s.Contains(CacheConstants.AdminDashboardCacheKey)),
                         Arg.Is<AdminChangeResultsViewModel>(m =>
                             m.MathsStatus == SubjectStatus.NotAchieved &&
                             m.MathsStatusTo == SubjectStatus.Achieved),
                         Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Redirected_To_Review_Page()
        {
            var redirectToRouteResult = Result.Should().BeOfType<RedirectToRouteResult>().Which;
            redirectToRouteResult.RouteName.Should().Be(RouteConstants.AdminReviewChangesLevelTwoMaths);
            redirectToRouteResult.RouteValues.Should().ContainKey("pathwayId");
            redirectToRouteResult.RouteValues["pathwayId"].Should().Be(ExpectedRegistrationPathwayId);
        }
    }
}