using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerUsedPost
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        public IpMultiEmployerUsedViewModel viewModel;
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.IpMultiEmployerUsedAsync(viewModel);
        }
    }
}
