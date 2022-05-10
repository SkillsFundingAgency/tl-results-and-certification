using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.AddIndustryPlacementAsync
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        public int ProfileId { get; set; }
        public IActionResult ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Controller.AddIndustryPlacementAsync(ProfileId);
        }
    }
}
