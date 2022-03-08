using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommCoreGradeGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }

        public bool? IsBack { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsAddRommCoreGradeAsync(ProfileId, AssessmentId, IsBack);
        }
    }
}
