using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationHoursGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AdminIndustryPlacementSpecialConsiderationHoursAsync();
        }
    }
}