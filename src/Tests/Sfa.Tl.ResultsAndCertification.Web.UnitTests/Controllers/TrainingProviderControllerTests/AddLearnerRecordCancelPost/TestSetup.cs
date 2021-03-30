using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddLearnerRecordCancelPost
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public LearnerRecordCancelViewModel LearnerRecordCancelViewModel;
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AddLearnerRecordCancelAsync(LearnerRecordCancelViewModel);
        }
    }
}
