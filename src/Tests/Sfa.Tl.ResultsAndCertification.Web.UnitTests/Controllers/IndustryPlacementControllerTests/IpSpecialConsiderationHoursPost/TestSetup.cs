using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationHoursPost
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public SpecialConsiderationHoursViewModel ViewModel { get; set; }

        public async override Task When()
        {
            Result = await Controller.IpSpecialConsiderationHoursAsync(ViewModel);
        }
    }
}
