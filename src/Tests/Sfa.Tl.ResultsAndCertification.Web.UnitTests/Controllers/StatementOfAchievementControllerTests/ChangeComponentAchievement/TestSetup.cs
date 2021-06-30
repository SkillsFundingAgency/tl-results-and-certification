using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.ChangeComponentAchievement
{
    public abstract class TestSetup : StatementOfAchievementControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId { get; set; }

        public async override Task When()
        {
            await Task.CompletedTask;
            Result = Controller.ChangeComponentAchievement(ProfileId);
        }
    }
}