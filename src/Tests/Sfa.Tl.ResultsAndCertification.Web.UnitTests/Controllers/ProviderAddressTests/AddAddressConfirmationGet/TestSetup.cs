using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressConfirmationGet
{
    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public string ConfirmationCacheKey { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AddAddressConfirmationAsync();
        }
    }
}
