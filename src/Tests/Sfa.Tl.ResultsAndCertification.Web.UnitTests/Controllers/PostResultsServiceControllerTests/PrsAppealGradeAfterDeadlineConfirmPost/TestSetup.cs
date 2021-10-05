using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealGradeAfterDeadlineConfirmPost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public AppealGradeAfterDeadlineConfirmViewModel ViewModel { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsAppealGradeAfterDeadlineConfirmAsync(ViewModel);
        }
    }
}
