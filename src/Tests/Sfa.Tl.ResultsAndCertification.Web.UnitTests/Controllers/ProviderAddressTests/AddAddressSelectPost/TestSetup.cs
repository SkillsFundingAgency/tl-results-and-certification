using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderAddressTests.AddAddressSelectPost
{
    public abstract class TestSetup : ProviderAddressControllerTestBase
    {
        public IActionResult Result { get; private set; }
        protected AddAddressSelectViewModel ViewModel {get;set;}

        public async override Task When()
        {
            Result = await Controller.AddAddressSelectAsync(ViewModel);
        }
    }
}