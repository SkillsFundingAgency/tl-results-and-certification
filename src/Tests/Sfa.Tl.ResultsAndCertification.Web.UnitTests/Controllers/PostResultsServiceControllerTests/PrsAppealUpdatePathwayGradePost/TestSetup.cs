using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAppealUpdatePathwayGradePost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public IActionResult Result { get; private set; }
        public AppealUpdatePathwayGradeViewModel ViewModel { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsAppealUpdatePathwayGradeAsync(ViewModel);
        }
    }
}
