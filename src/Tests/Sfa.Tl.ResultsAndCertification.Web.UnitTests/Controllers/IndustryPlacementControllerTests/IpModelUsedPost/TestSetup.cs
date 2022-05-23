using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedPost
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        public int ProfileId { get; set; }
        public IpModelUsedViewModel ViewModel;
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.IpModelUsedAsync(ViewModel);
        }
    }
}
