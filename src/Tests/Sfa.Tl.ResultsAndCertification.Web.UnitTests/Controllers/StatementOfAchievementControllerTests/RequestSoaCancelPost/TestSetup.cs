using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaCancelPost
{
    public abstract class TestSetup : StatementOfAchievementControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public RequestSoaCancelViewModel ViewModel;

        public async override Task When()
        {
            await Task.CompletedTask;
            Result = Controller.RequestSoaCancelAsync(ViewModel);
        }
    }
}