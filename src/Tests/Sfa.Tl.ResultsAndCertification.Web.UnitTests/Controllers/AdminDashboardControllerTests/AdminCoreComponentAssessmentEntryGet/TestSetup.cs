using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminCoreComponentAssessmentEntryGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {   
        protected AdminCoreComponentViewModel AdminCoreComponentViewModel;
        public IActionResult Result { get; private set; }
        protected int RegistrationPathwayId { get; set; }

        public async override Task When()
        {
            Result = await Controller.AdminCoreComponentAssessmentEntry(RegistrationPathwayId);
        }
    }
}
