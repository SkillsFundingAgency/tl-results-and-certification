using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.StatementsOfAchievementNotAvailableGet
{
    public abstract class TestSetup : StatementOfAchievementControllerTestBase
    {
        public IActionResult Result { get; private set; }

        public override Task When()
        {
            Result = Controller.StatementsOfAchievementNotAvailable();
            return Task.CompletedTask;
        }
    }
}