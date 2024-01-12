using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminIpCompletionViewModel AdminChangeIndustryPlacementViewModel = null;
        
        protected IActionResult Result { get; private set; }

        protected int RegistrationPathwayId { get; set; }

        public async override Task When()
        {
            Result = await Controller.ChangeIndustryPlacementAsync(RegistrationPathwayId);
        }
    }
}
