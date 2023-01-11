using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionGet
{
    public abstract class TestSetup : IndustryPlacementControllerTestBase
    {
        public int ProfileId { get; set; }
        public int PathwayId { get; set; }
        public bool IsChangeMode { get; set; }
        public IActionResult Result { get; private set; }

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

        public async override Task When()
        {
            Result = await Controller.IpCompletionAsync(ProfileId, IsChangeMode);
        }
    }
}
