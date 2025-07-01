using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectsStatus;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesMathsStatusPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        public IActionResult ActualResult { get; set; }
        protected AdminReviewChangesMathsStatusViewModel ViewModel;

        public async override Task When()
        {
            ActualResult = await Controller.AdminReviewChangesMathsStatusAsync(ViewModel);
        }

        protected AdminReviewChangesMathsStatusViewModel CreateViewModel(SubjectStatus? mathsStatus)
        {
            return new AdminReviewChangesMathsStatusViewModel
            {
                ChangeReason = "change-reason",
                ContactName = "contact-name",
                ZendeskId = "1234567890",
                AdminChangeStatusViewModel = new AdminChangeMathsStatusViewModel()
                {
                    RegistrationPathwayId = 1,
                    MathsStatusTo = mathsStatus
                }
            };
        }
    }
}