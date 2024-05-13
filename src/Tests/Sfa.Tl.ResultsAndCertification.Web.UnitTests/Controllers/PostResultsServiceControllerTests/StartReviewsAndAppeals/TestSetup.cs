using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.StartReviewsAndAppeals
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }

        public override async Task When()
        {
            Result = await Controller.StartReviewsAndAppealsAsync();
        }
    }
}
