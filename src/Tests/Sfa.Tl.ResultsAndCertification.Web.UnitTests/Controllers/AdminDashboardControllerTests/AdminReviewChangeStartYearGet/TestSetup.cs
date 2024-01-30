using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeStartYearGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {   
        protected ReviewChangeStartYearViewModel ReviewChangeStartYearViewModel;
        public IActionResult Result { get; private set; }
        protected int PathwayId { get; set; }

        public async override Task When()
        {
            Result = await Controller.ReviewChangeStartYearAsync(PathwayId);
        }
    }
}
