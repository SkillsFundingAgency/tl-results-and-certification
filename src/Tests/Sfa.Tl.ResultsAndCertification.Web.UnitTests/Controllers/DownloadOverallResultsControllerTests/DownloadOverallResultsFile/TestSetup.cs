using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests.DownloadOverallResultsFile
{
    public abstract class TestSetup : DownloadResultsControllerTestBase
    {
        public IActionResult Result { get; set; }
        protected override DateTime CurrentDate => DateTime.UtcNow.AddDays(-1);


        public async override Task When()
        {
            Result = await Controller.DownloadOverallResultsFileAsync();
        }
    }
}
