using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSearchLearnerPost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public PrsSearchLearnerViewModel ViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.PrsSearchLearnerAsync(ViewModel);
        }
    }
}