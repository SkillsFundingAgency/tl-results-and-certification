using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewRemoveSpecialismAssessmentEntryGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {   
        protected AdminReviewRemoveSpecialismAssessmentEntryViewModel ReviewRemoveSpecialismAssessmentEntryViewModel;
        public IActionResult Result { get; private set; }
        protected int PathwayId { get; set; }

        public async override Task When()
        {
            Result = await Controller.AdminReviewRemoveSpecialismAssessmentEntryAsync(PathwayId);
        }
    }
}
