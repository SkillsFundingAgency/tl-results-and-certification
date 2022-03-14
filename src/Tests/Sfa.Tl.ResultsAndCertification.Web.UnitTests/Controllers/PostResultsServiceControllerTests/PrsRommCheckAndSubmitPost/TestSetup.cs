using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsRommCheckAndSubmitPost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public PrsRommCheckAndSubmitViewModel ViewModel;
        public IActionResult Result;

        public async override Task When()
        {
            Result = await Controller.PrsRommCheckAndSubmitAsync(ViewModel);
        }
    }
}