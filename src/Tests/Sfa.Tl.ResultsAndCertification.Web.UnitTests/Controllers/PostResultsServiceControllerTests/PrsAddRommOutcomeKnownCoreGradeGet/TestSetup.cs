using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownCoreGradeGet
{
    public abstract class TestSetup : PostResultsServiceControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }

        public int? RommOutcomeTypeId { get; set; }

        public async override Task When()
        {
            Result = await Controller.PrsAddRommOutcomeKnownCoreGradeAsync(ProfileId, AssessmentId, RommOutcomeTypeId);
        }
    }
}