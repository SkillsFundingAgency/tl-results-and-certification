using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommCoreGradePost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public IActionResult Result { get; private set; }
        public PrsAddRommCoreGradeViewModel ViewModel { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsAddRommCoreGradeAsync(ViewModel);
        }
    }
}
