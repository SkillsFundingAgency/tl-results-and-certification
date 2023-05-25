using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SubmitWithdrawnStatusPost
{
    public abstract class TestSetup: TrainingProviderControllerTestBase
    {
        public WithdrawnConfirmationViewModel ViewModel;
        public int ProfileId;
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.SubmitWithdrawnStatusAsync(ViewModel);
        }
    }
}