using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddPostalAddressManualGet
{
    // IsSelect	    IsMissing		RouteName			RouteAttriCount		
    // Y			Y				AddAddressSelect	1								IsAddressMissing
    // N			Y				AddAddressPostcode	2				ShowPostcode,	IsAddressMissing
    // N			N				AddAddressPostcode	1				ShowPostcode
    // Y			N				AddAddressSelect	0	

    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public bool IsFromSelectAddress { get; set; }
        public bool IsFromAddressMissing { get; set; }

        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AddPostalAddressManualAsync(IsFromSelectAddress, IsFromAddressMissing);
        }
    }
}
