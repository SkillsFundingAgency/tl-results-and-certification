using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesEnglishStatusPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        public IActionResult ActualResult { get; set; }
        protected AdminReviewChangesEnglishStatusViewModel ViewModel;

        public async override Task When()
        {
            ActualResult = await Controller.AdminReviewChangesEnglishStatusAsync(ViewModel);
        }

        protected AdminReviewChangesEnglishStatusViewModel CreateViewModel(SubjectStatus? englishStatus)
        {
            return new AdminReviewChangesEnglishStatusViewModel
            {
                ChangeReason = "change-reason",
                ContactName = "contact-name",
                ZendeskId = "1234567890",
                AdminChangeStatusViewModel = new AdminChangeEnglishStatusViewModel()
                {
                    RegistrationPathwayId = 1,
                    EnglishStatusTo = englishStatus
                }
            };
        }
    }
}