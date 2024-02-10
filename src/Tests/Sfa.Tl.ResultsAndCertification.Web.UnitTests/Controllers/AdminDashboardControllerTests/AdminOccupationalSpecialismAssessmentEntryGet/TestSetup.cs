using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminOccupationalSpecialismAssessmentEntry
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {   
        protected AdminOccupationalSpecialismViewModel AdminOccupationalSpecialismViewModel;
        public IActionResult Result { get; private set; }
        public int SpecialismId { get; set; }
        protected int RegistrationPathwayId { get; set; }

        public async override Task When()
        {
            Result = await Controller.AdminOccupationalSpecialismAssessmentEntry(RegistrationPathwayId, SpecialismId);
        }
    }
}
