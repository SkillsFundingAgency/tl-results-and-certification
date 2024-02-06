using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.LearnerRecordGet
{
    public class When_Called_With_Invalid_Data: AdminDashboardControllerTestBase
    {
        private const int RegistrationPathwayId = 1;
        private IActionResult Result;

        public override void Given()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(RegistrationPathwayId).Returns(null as AdminLearnerRecordViewModel);
        }

        public async override Task When()
        {
            Result = await Controller.AdminLearnerRecordAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<AdminLearnerRecordViewModel>(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}