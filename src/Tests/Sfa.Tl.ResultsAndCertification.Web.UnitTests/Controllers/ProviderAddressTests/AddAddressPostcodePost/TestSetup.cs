using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressPostcodePost
{
    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public IActionResult Result { get; private set; }

        public AddAddressPostcodeViewModel ViewModel;

        public async override Task When()
        {
            Result = await Controller.AddAddressPostcodeAsync(ViewModel);
        }
    }
}
