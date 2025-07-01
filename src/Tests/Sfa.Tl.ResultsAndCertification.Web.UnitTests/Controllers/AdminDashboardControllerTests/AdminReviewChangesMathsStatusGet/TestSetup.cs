using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesMathsStatusGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminReviewChangesMathsStatusViewModel ReviewChangesMathsStatusViewModel;
        public IActionResult Result { get; private set; }
        protected int PathwayId { get; set; }

        public async override Task When()
        {
            Result = await Controller.AdminReviewChangesMathsStatusAsync(PathwayId);
        }
    }
}