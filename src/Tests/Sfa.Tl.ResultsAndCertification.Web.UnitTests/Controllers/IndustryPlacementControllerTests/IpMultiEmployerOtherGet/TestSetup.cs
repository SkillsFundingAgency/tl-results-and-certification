using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerOtherGet
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public bool IsChangeMode { get; set; }
        public async override Task When()
        {
            Result = await Controller.IpMultiEmployerOtherAsync(IsChangeMode);
        }
    }
}
