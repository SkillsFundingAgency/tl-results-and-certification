using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddMathsStatusPost
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public int ProfileId { get; set; }
        public AddMathsStatusViewModel ViewModel { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AddMathsStatusAsync(ViewModel);
        }
    }
}
