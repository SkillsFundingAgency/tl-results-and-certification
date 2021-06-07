using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaConfirmation
{
    public abstract class TestSetup : StatementOfAchievementControllerTestBase
    {
        protected string ConfirmationCacheKey;
        public IActionResult Result { get; set; }
        protected SoaConfirmationViewModel SoaConfirmationViewModel;

        public async override Task When()
        {
            Result = await Controller.RequestSoaConfirmationAsync();
        }
    }
}
