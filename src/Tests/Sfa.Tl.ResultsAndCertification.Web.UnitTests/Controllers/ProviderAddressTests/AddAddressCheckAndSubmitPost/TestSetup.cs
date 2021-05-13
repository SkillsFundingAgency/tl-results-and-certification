using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressCheckAndSubmitPost
{
    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public IActionResult Result { get; private set; }
        protected AddAddressViewModel AddAddressViewModel;
        public AddAddressPostcodeViewModel AddAddressPostcode { get; set; }
        public AddAddressSelectViewModel AddAddressSelect { get; set; }
        public AddAddressManualViewModel AddAddressManual { get; set; }

        public async override Task When()
        {
            Result = await Controller.SubmitAddAddressCheckAndSubmitAsync();
        }
    }
}