using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddPostalAddressManualGet
{
    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public bool IsFromSelectAddress { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AddPostalAddressManualAsync(IsFromSelectAddress);
        }
    }
}
