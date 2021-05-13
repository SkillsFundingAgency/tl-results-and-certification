using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressCancelPost
{
    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public AddAddressCancelViewModel ViewModel;
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AddAddressCancelAsync(ViewModel);
        }
    }
}
