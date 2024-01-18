using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ReviewChangeStartYearPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected int AoUkprn;
        protected int ProfileId;
        protected ReviewChangeStartYearViewModel ReviewChangeStartYearViewModel;

        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.ReviewChangeStartYearAsync(ReviewChangeStartYearViewModel);
        }
    }
}
