using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddLearnerRecordConfirmation
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        protected string ConfirmationCacheKey;
        public IActionResult Result { get; set; }
        protected LearnerRecordConfirmationViewModel LearnerRecordConfirmationViewModel;

        public async override Task When()
        {
            Result = await Controller.AddLearnerRecordConfirmationAsync();
        }
    }
}
