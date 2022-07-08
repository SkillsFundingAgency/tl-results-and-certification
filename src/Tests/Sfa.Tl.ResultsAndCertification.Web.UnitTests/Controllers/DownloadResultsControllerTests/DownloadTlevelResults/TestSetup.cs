using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadResultsControllerTests.DownloadTlevelResults
{
    public abstract class TestSetup : DownloadResultsControllerTestBase
    {
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = Controller.DownloadTlevelResults();
            await Task.CompletedTask;
        }
    }
}
