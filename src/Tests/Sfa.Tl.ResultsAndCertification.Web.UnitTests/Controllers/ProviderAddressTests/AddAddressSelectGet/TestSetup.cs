using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressSelectGet
{
    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AddAddressSelectAsync();
        }
    }
}
