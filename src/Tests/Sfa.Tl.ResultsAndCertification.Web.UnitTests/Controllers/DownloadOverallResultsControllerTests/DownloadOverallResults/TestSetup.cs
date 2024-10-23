using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests.DownloadOverallResults
{
    public abstract class TestSetup : DownloadResultsControllerTestBase
    {
        public IActionResult Result { get; set; }

        public async override Task When()
        {
            Result = Controller.DownloadOverallResults();
            await Task.CompletedTask;
        }
    }
}
