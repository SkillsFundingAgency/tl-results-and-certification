using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests.DownloadOverallResultsFile
{
    public abstract class TestSetup : DownloadResultsControllerTestBase
    {
        public IActionResult Result { get; set; }

        public async override Task When()
        {
            Result = await Controller.DownloadOverallResultsFileAsync();
        }
    }
}
