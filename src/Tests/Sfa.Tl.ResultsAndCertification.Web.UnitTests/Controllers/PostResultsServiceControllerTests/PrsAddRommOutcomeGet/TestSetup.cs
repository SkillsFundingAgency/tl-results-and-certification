using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }

        public int? RommOutcomeTypeId { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsAddRommOutcomeAsync(ProfileId, AssessmentId, RommOutcomeTypeId);
        }
    }
}