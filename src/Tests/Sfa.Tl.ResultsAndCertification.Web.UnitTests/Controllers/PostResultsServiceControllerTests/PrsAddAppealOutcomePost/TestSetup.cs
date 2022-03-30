using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddAppealOutcomePost
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public ComponentType ComponentType { get; set; }
        public IActionResult Result { get; private set; }
        public PrsAddAppealOutcomeViewModel ViewModel { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsAddAppealOutcomeAsync(ViewModel);
        }
    }
}
