using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeUlnClear
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        public IActionResult Result { get; private set; }

        public override async Task When()
        {
            Result = await Controller.ChangeUlnClearAsync(3);
        }
    }
}
