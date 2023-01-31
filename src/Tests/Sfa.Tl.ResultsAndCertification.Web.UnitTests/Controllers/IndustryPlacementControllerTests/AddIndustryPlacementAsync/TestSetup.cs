using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
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

        public void SetRouteAttribute(string routeName)
        {
            Controller.ControllerContext = new ControllerContext
            {
                HttpContext = HttpContext,
                ActionDescriptor = new ControllerActionDescriptor
                {
                    AttributeRouteInfo = new AttributeRouteInfo
                    {
                        Name = routeName
                    }
                }
            };
        }
    }
}
