using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateEnglisAndMathsAchievementPost
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        protected int ProfileId;
        protected UpdateEnglishAndMathsQuestionViewModel ViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.UpdateEnglisAndMathsAchievementAsync(ViewModel);
        }
    }
}
