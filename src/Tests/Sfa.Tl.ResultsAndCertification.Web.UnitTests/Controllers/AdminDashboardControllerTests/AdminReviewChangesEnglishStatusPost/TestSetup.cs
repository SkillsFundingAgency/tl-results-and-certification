using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesEnglishStatusPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        public IActionResult ActualResult { get; set; }
        protected AdminReviewChangesEnglishSubjectViewModel ViewModel;

        public async override Task When()
        {
            ActualResult = await Controller.AdminReviewChangesEnglishStatusAsync(ViewModel);
        }

        protected AdminReviewChangesEnglishSubjectViewModel CreateViewModel(SubjectStatus? englishStatus)
        {
            return new AdminReviewChangesEnglishSubjectViewModel
            {
                ChangeReason = "change-reason",
                ContactName = "contact-name",
                ZendeskId = "1234567890",
                AdminChangeResultsViewModel = new AdminChangeEnglishResultsViewModel()
                {
                    RegistrationPathwayId = 1,
                    EnglishStatusTo = englishStatus
                }
            };
        }
    }
}