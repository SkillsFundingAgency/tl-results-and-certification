using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeBackToActiveAOMessagePost
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public int ProfileId { get; set; }
        public ChangeBackToActiveAOMessageViewModel ViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public override Task When()
        {
            Result = Controller.ChangeBackToActiveAOMessageAsync(ViewModel);
            return Task.CompletedTask;
        }
    }
}