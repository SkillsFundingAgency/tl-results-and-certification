using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderRegistrationsControllerTests.DownloadRegistrationsDataForLink
{
    public abstract class TestSetup : ProviderRegistrationsControllerTestBase
    {
        protected IActionResult Result;

        public async Task When(string id)
        {
            Result =  await Controller.DownloadRegistrationsDataForLinkAsync(id);
        }
    }
}