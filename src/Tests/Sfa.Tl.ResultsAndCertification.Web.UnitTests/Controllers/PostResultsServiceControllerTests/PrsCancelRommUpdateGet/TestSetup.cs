using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelRommUpdateGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public ComponentType ComponentType { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.PrsCancelRommUpdateAsync();
        }
    }
}
