using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.RequestSoaUniqueLearnerNumberPost
{
    public abstract class TestSetup : StatementOfAchievementControllerTestBase
    {
        public RequestSoaUniqueLearnerNumberViewModel ViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.RequestSoaUniqueLearnerNumberAsync(ViewModel);
        }
    }
}
