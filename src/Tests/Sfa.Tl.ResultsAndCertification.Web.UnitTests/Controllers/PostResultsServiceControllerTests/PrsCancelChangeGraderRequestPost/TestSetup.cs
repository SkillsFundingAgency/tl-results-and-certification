using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelChangeGraderRequestPost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public PrsCancelGradeChangeRequestViewModel ViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = Controller.PrsCancelGradeChangeRequest(ViewModel);
            await Task.CompletedTask;
        }
    }
}
