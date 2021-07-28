using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelAppealUpdatePost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public PrsCancelAppealUpdateViewModel ViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.PrsCancelAppealUpdateAsync(ViewModel);
        }
    }
}
