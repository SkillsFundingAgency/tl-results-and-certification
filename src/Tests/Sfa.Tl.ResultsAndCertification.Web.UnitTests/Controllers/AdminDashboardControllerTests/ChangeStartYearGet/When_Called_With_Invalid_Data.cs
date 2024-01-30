using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System.Threading.Tasks;
using Xunit;
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeStartYearGet
{
    public class When_Called_With_Invalid_Data: AdminDashboardControllerTestBase
    {
        public int PathwayId { get; set; }
        protected AdminChangeStartYearViewModel Mockresult = null;
        public IActionResult Result { get; private set; }


        public override void Given()
        {
            PathwayId = 0;
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(PathwayId).Returns(Mockresult);
        }

        public async override Task When()
        {
            Result = await Controller.AdminLearnerRecordAsync(PathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(PathwayId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
