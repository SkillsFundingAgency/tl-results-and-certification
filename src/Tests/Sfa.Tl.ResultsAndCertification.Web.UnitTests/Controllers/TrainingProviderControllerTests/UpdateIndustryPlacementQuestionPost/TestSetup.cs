using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.UpdateIndustryPlacementQuestionPost
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        protected int ProfileId;
        protected int PathwayId;
        protected UpdateIndustryPlacementQuestionViewModel UpdateIndustryPlacementQuestionViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.UpdateIndustryPlacementQuestionAsync(UpdateIndustryPlacementQuestionViewModel);
        }
    }
}
