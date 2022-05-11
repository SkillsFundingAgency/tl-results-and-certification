using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpModelUsedGet
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        protected bool IsChangeMode { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.IpModelUsedAsync(IsChangeMode);
        }
    }
}
