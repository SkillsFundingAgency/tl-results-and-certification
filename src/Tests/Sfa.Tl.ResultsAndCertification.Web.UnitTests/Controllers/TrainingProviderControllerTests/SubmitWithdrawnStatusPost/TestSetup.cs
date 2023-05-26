using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SubmitWithdrawnStatusPost
{
    public abstract class TestSetup: TrainingProviderControllerTestBase
    {
        public AddWithdrawnStatusViewModel ViewModel;
        public int ProfileId;
        public IActionResult Result { get; private set; }

        public override Task When()
        {
            Result = Controller.AddWithdrawnStatusAsync(ViewModel);
            return Task.CompletedTask;
        }
    }
}