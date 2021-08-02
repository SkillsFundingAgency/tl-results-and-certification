using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsGradeChangeRequestGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public bool IsResultJourney { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsGradeChangeRequestAsync(ProfileId, AssessmentId, IsResultJourney);
        }
    }
}
