using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionGet
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        public int ProfileId { get; set; }
        public int PathwayId { get; set; }
        public bool IsChangeMode { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.IpCompletionAsync(ProfileId, IsChangeMode);
        }
    }
}
