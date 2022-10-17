using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.RequestReplacementDocumentGet
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public int ProfileId;

        public async override Task When()
        {
            Result = await Controller.RequestReplacementDocumentAsync(ProfileId);
        }
    }
}
