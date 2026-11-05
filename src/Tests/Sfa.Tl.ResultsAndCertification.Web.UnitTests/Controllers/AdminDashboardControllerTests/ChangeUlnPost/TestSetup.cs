using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeUlnPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminChangeUlnViewModel Model;

        public IActionResult Result { get; private set; }

        public override async Task When()
        {
            Result = await Controller.ChangeUlnAsync(3, Model);
        }

        protected AdminChangeUlnViewModel Entry() => new()
        {
            RegistrationPathwayId = 3, Uln = 9999999990, UlnTo = "9999999991"
        };
    }
}
