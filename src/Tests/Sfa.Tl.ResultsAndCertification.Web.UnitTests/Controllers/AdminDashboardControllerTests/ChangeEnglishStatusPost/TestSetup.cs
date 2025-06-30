using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeEnglishStatusPost
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected AdminChangeEnglishResultsViewModel ViewModel;

        protected IActionResult Result;

        public override async Task When()
        {
            Result = await Controller.AdminChangeEnglishStatusAsync(ViewModel);
        }

        protected AdminChangeEnglishResultsViewModel CreateViewModel(int registrationPathwayId, SubjectStatus? originalStatus, SubjectStatus? statusTo)
        {
            return new AdminChangeEnglishResultsViewModel
            {
                RegistrationPathwayId = registrationPathwayId,
                LearnerName = "Frank West",
                Uln = 1234567890,
                Provider = "Barnsley College (10000536)",
                TlevelName = "Education and Early Years",
                AcademicYear = 2020,
                StartYear = "2021 to 2022",
                EnglishStatus = originalStatus,
                EnglishStatusTo = statusTo
            };
        }
    }
}
