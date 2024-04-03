using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddRommOutcomeChangeGradeCorePost
{
    public abstract class TestSetup : AdminPostResultsControllerTestBase
    {
        protected const int RegistrationPathwayId = 1, AssessmentId = 1;
        protected IActionResult Result;

        protected static AdminAddRommOutcomeChangeGradeCoreViewModel CreateViewModel(int? selectedGradeId = null)
            => new()
            {
                RegistrationPathwayId = RegistrationPathwayId,
                PathwayAssessmentId = AssessmentId,
                PathwayName = "Healthcare Science (6037083X)",
                Learner = "John Smith",
                Uln = 1080808080,
                Provider = "Barnsley College (10000536)",
                Tlevel = "Healthcare Science",
                StartYear = "2023 to 2024",
                ExamPeriod = "Summer 2024",
                Grade = string.Empty,
                SelectedGradeId = selectedGradeId
            };
    }
}