using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelRommUpdatePost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public PrsCancelRommUpdateViewModel ViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.PrsCancelRommUpdateAsync(ViewModel);
        }
    }
}
