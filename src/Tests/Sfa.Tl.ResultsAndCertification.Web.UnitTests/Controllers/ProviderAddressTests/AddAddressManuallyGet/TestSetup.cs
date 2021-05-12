using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressManuallyGet
{
    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public bool IsFromSelectAddress { get; set; }

        public async override Task When()
        {
            Result = await Controller.AddAddressManuallyAsync(IsFromSelectAddress);
        }
    }
}
