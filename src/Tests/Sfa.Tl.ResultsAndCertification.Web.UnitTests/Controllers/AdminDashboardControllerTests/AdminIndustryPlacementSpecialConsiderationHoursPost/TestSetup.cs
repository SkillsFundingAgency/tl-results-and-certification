using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationHoursPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminIpSpecialConsiderationHoursViewModel ViewModel;

        protected IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AdminIndustryPlacementSpecialConsiderationHoursAsync(ViewModel);
        }
    }
}