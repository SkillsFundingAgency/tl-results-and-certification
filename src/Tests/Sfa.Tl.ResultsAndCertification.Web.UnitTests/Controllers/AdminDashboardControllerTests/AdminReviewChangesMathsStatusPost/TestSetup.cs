using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesMathsStatusPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        public IActionResult ActualResult { get; set; }
        protected AdminReviewChangesMathsSubjectViewModel ViewModel;

        public async override Task When()
        {
            ActualResult = await Controller.AdminReviewChangesMathsStatusAsync(ViewModel);
        }

        protected AdminReviewChangesMathsSubjectViewModel CreateViewModel(SubjectStatus? mathsStatus)
        {
            return new AdminReviewChangesMathsSubjectViewModel
            {
                ChangeReason = "change-reason",
                ContactName = "contact-name",
                ZendeskId = "1234567890",
                AdminChangeResultsViewModel = new AdminChangeMathsResultsViewModel()
                {
                    RegistrationPathwayId = 1,
                    MathsStatusTo = mathsStatus
                }
            };
        }
    }
}