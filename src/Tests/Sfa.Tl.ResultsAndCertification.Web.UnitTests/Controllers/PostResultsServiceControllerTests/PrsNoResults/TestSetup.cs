using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsNoResults
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        protected int ProfileId = 1;

        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.PrsNoResultsAsync(ProfileId);
        }
    }
}