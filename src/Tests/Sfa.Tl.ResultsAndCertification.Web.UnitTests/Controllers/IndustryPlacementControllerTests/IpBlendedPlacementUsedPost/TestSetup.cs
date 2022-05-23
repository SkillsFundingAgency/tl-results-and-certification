using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpBlendedPlacementUsedPost
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public IpBlendedPlacementUsedViewModel ViewModel { get; set; }

        public async override Task When()
        {
            Result = await Controller.IpBlendedPlacementUsedAsync(ViewModel);
        }
    }
}
