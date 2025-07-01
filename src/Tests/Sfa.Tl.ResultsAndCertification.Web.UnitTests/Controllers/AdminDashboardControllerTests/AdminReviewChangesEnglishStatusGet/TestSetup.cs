using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesEnglishStatusGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminReviewChangesEnglishSubjectViewModel ReviewChangesEnglishSubjectViewModel;
        public IActionResult Result { get; private set; }
        protected int PathwayId { get; set; }

        public async override Task When()
        {
            Result = await Controller.AdminReviewChangesEnglishStatusAsync(PathwayId);
        }
    }
}