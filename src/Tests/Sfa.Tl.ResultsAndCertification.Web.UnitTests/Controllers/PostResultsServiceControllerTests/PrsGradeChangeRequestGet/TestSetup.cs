using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsGradeChangeRequestGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public Common.Enum.ComponentType ComponentType { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsGradeChangeRequestAsync(ProfileId, AssessmentId, ComponentType);
        }
    }
}
