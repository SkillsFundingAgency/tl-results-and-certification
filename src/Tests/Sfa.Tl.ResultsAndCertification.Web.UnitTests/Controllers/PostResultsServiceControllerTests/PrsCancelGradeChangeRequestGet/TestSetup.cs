using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsCancelGradeChangeRequestGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ComponentType { get; set; }
        public bool IsResultJourney { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsCancelGradeChangeRequestAsync(ProfileId, AssessmentId, ComponentType, IsResultJourney);
        }
    }
}
