using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateEnglisAndMathsAchievementGet
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        protected int ProfileId;
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.UpdateEnglisAndMathsAchievementAsync(ProfileId);
        }
    }
}
