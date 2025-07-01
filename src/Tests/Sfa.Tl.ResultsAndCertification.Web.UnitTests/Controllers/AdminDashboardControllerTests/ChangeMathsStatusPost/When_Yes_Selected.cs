using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeMathsStatusPost
{
    public class When_Yes_Selected : TestSetup
    {
        private const int ExpectedRegistrationPathwayId = 1;
        private AdminChangeMathsResultsViewModel _originalViewModel;

        public override void Given()
        {
            ViewModel = CreateViewModel(ExpectedRegistrationPathwayId, null, SubjectStatus.Achieved);

            // Set up the original view model that will be returned by the loader
            _originalViewModel = CreateViewModel(ExpectedRegistrationPathwayId, SubjectStatus.NotAchieved, null);

            AdminDashboardLoader
                .GetAdminLearnerRecordAsync<AdminChangeMathsResultsViewModel>(ExpectedRegistrationPathwayId)
                .Returns(_originalViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            AdminDashboardLoader
                .Received(1)
                .GetAdminLearnerRecordAsync<AdminChangeMathsResultsViewModel>(ExpectedRegistrationPathwayId);

            CacheService
                .Received(1)
                .SetAsync(Arg.Is<string>(s => s.Contains(CacheConstants.AdminDashboardCacheKey)),
                         Arg.Is<AdminChangeMathsResultsViewModel>(m =>
                             m.MathsStatus == SubjectStatus.NotAchieved &&
                             m.MathsStatusTo == SubjectStatus.Achieved),
                         Arg.Any<CacheExpiryTime>());
        }

        [Fact]
        public void Then_Redirected_To_Review_Page()
        {
            var redirectToRouteResult = Result.Should().BeOfType<RedirectToRouteResult>().Which;
            redirectToRouteResult.RouteName.Should().Be(RouteConstants.AdminReviewChangesMathsStatus);
            redirectToRouteResult.RouteValues.Should().ContainKey("pathwayId");
            redirectToRouteResult.RouteValues["pathwayId"].Should().Be(ExpectedRegistrationPathwayId);
        }
    }
}