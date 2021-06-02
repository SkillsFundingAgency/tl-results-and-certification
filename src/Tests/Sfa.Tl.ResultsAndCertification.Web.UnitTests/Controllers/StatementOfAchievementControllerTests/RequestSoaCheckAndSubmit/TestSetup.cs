using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCheckAndSubmit
{
    public abstract class TestSetup : StatementOfAchievementControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }

        public async override Task When()
        {
            Result = await Controller.RequestSoaCheckAndSubmitAsync(ProfileId);
        }
    }
}
