using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderRegistrationsControllerTests.DownloadRegistrationsDataFor
{
    public abstract class TestSetup : ProviderRegistrationsControllerTestBase
    {
        protected const int StartYear = 2020;

        protected IActionResult Result;

        public override async Task When()
        {
            Result =  await Controller.DownloadRegistrationsDataForAsync(StartYear);
        }
    }
}