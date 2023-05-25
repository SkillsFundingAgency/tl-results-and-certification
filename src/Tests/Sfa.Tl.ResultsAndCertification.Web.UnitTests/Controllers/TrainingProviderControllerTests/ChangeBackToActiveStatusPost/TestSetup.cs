using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.ChangeBackToActiveStatusPost
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public ChangeBackToActiveStatusViewModel ViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public override Task When()
        {
            Result = Controller.ChangeBackToActiveStatusAsync(ViewModel);
            return Task.CompletedTask;
        }
    }
}