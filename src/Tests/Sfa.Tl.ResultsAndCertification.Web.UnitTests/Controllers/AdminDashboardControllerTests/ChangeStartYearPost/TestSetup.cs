using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeStartYearPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminChangeStartYearViewModel AdminChangeStartYearViewModel;

        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.ChangeStartYearAsync(AdminChangeStartYearViewModel);
        }
    }
}
