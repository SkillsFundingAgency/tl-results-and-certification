using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeMathsStatusPost
{
    public class When_Yes_Selected_Lrs : TestSetup
    {
        private const int ExpectedRegistrationPathwayId = 1;
        private AdminChangeMathsStatusViewModel _originalViewModel;

        public override void Given()
        {
            ViewModel = CreateViewModel(ExpectedRegistrationPathwayId, null, SubjectStatus.Achieved);

            // Set up the original view model that will be returned by the loader
            _originalViewModel = CreateViewModel(ExpectedRegistrationPathwayId, SubjectStatus.NotAchievedByLrs, null);

            AdminDashboardLoader
                .GetAdminLearnerRecordAsync<AdminChangeMathsStatusViewModel>(ExpectedRegistrationPathwayId)
                .Returns(_originalViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            AdminDashboardLoader
                .Received(1)
                .GetAdminLearnerRecordAsync<AdminChangeMathsStatusViewModel>(ExpectedRegistrationPathwayId);

            CacheService
                .Received(1)
                .SetAsync(Arg.Is<string>(s => s.Contains(CacheConstants.AdminDashboardCacheKey)),
                         Arg.Is<AdminChangeMathsStatusViewModel>(m =>
                             m.MathsStatus == SubjectStatus.NotAchievedByLrs &&
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