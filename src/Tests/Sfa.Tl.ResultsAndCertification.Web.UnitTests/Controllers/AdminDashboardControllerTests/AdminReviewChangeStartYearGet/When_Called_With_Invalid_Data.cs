using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeStartYearGet
{
    public class When_Called_With_Invalid_Data: TestSetup
    {
        public int PathwayId { get; set; }
        protected AdminChangeStartYearViewModel AdminChangeStartYearViewModel = null;
        protected ReviewChangeStartYearViewModel Mockresult = null;
        public IActionResult Result { get; private set; }


        public override void Given()
        {
            PathwayId = 0;
            AdminDashboardLoader.GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(PathwayId).Returns(Mockresult);
            CacheService.GetAsync<AdminChangeStartYearViewModel>(CacheKey).Returns(AdminChangeStartYearViewModel);
        }

        public async override Task When()
        {
            Result = await Controller.ReviewChangeStartYearAsync(PathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(PathwayId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
