using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.EnterUniqueLearnerReferenceGet
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        protected bool IsNavigatedFromSearchLearnerRecordNotAdded;
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.EnterUniqueLearnerReferenceAsync(IsNavigatedFromSearchLearnerRecordNotAdded);
        }
    }
}
