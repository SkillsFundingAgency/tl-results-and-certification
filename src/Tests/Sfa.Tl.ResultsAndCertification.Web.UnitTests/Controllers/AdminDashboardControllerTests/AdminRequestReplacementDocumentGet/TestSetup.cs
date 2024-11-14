using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminRequestReplacementDocumentGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected int RegistrationPathwayId = 150;

        protected IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AdminRequestReplacementDocumentAsync(RegistrationPathwayId);
        }
    }
}