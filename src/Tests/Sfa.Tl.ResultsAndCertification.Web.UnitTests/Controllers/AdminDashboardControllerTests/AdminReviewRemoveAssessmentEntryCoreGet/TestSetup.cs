using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewRemoveAssessmentEntryCoreGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {   
        protected AdminReviewRemoveCoreAssessmentEntryViewModel ReviewRemoveCoreAssessmentEntryViewModel;
        public IActionResult Result { get; private set; }
        protected int PathwayId { get; set; }

        public async override Task When()
        {
            Result = await Controller.AdminReviewRemoveCoreAssessmentEntryAsync(PathwayId);
        }
    }
}
