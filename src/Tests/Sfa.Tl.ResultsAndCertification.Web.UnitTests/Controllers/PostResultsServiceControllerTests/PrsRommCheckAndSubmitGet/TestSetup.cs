using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsRommCheckAndSubmitGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public ComponentType ComponentType { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsRommCheckAndSubmitAsync();
        }
    }
}
