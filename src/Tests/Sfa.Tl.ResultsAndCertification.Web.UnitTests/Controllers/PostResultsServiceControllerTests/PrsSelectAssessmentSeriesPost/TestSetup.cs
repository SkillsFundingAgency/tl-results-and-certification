using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsSelectAssessmentSeriesPost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public PrsSelectAssessmentSeriesViewModel ViewModel { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsSelectAssessmentSeriesAsync(ViewModel);
        }
    }
}