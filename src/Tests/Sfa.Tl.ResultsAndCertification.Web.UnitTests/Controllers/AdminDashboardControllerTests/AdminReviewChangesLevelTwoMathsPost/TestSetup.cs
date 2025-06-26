using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesLevelTwoMathsPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        public IActionResult ActualResult { get; set; }
        protected AdminReviewChangesLevelTwoMathsViewModel ViewModel;

        public async override Task When()
        {
            ActualResult = await Controller.AdminReviewChangesLevelTwoMathsAsync(ViewModel);
        }

        protected AdminReviewChangesLevelTwoMathsViewModel CreateViewModel(SubjectStatus? mathsStatus)
        {
            return new AdminReviewChangesLevelTwoMathsViewModel
            {
                ChangeReason = "change-reason",
                ContactName = "contact-name",
                ZendeskId = "1234567890",
                AdminChangeResultsViewModel = new AdminChangeResultsViewModel()
                {
                    RegistrationPathwayId = 1,
                    MathsStatusTo = mathsStatus
                }
            };
        }
    }
}