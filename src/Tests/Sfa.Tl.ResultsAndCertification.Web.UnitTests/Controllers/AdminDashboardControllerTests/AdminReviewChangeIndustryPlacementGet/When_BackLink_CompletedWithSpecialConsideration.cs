using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeIndustryPlacementGet
{
    public class When_BackLink_CompletedWithSpecialConsideration : TestSetup
    {
        protected AdminReviewChangesIndustryPlacementViewModel Mockresult = null;

        public override void Given()
        {
            AdminIpCompletionViewModel adminIpCompletionViewModel = new()
            {
                Uln = 1235469874,
                IndustryPlacementStatusTo = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration
            };

            AdminChangeIpViewModel adminChangeIpViewModel = new()
            {
                AdminIpCompletion = adminIpCompletionViewModel,
                HoursViewModel = new(),
                ReasonsViewModel = new()
            };

            Mockresult = new AdminReviewChangesIndustryPlacementViewModel
            {
                AdminChangeIpViewModel = adminChangeIpViewModel
            };

            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminReviewChangesIndustryPlacementViewModel>(PathwayId).Returns(Mockresult);
            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(adminChangeIpViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as AdminReviewChangesIndustryPlacementViewModel;

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AdminIndustryPlacementSpecialConsiderationReasons);
        }
    }
}
