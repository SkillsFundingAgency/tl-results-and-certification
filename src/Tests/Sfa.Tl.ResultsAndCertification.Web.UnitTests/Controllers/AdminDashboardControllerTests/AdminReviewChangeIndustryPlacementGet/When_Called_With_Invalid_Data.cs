using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeIndustryPlacementGet
{
    public class When_Called_With_Invalid_Data: TestSetup
    {
        protected AdminReviewChangesIndustryPlacementViewModel Mockresult = null;

        public override void Given()
        {
            PathwayId = 0;
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminReviewChangesIndustryPlacementViewModel>(PathwayId).Returns(Mockresult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
