using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewRemoveAssessmentEntryCorePost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminReviewRemoveCoreAssessmentEntryViewModel ReviewRemoveCoreAssessmentEntryViewModel;

        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AdminReviewRemoveCoreAssessmentEntryAsync(ReviewRemoveCoreAssessmentEntryViewModel);
        }
    }
}
