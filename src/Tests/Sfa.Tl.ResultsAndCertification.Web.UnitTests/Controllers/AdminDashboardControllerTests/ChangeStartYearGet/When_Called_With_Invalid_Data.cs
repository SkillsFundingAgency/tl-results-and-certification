using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeStartYearGet
{
    public class When_Called_With_Invalid_Data : AdminDashboardControllerTestBase
    {
        private const int RegistrationPathwayId = 1;
        private IActionResult Result;

        public override void Given()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(RegistrationPathwayId).Returns(null as AdminChangeStartYearViewModel);
        }

        public async override Task When()
        {
            Result = await Controller.ChangeStartYearAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminChangeStartYearViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}