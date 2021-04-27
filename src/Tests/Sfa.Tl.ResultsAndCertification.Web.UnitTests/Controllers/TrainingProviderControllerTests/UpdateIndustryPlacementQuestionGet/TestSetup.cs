using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateIndustryPlacementQuestionGet
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        protected int ProfileId;
        protected int PathwayId;
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.UpdateIndustryPlacementQuestionAsync(ProfileId, PathwayId);
        }
    }
}