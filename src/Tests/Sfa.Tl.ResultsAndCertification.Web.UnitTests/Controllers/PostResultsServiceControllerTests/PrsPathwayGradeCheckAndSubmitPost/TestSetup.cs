using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsPathwayGradeCheckAndSubmitPost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public PrsPathwayGradeCheckAndSubmitViewModel ViewModel;
        public IActionResult Result;

        public async override Task When()
        {
            Result = await Controller.PrsPathwayGradeCheckAndSubmitAsync(ViewModel);
        }
    }
}
