using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddRommOutcomeChangeGradeSpecialismGet

{
    public abstract class TestSetup : AdminPostResultsControllerTestBase
    {
        protected const int RegistrationPathwayId = 1, AssessmentId = 1;

        protected IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AdminAddRommOutcomeChangeGradeSpecialismAsync(RegistrationPathwayId, AssessmentId);
        }

        protected static AdminAddRommOutcomeChangeGradeSpecialismViewModel ViewModel
            => new()
            {
                RegistrationPathwayId = RegistrationPathwayId,
                SpecialismAssessmentId = AssessmentId,
                SpecialismName = "Plastering (ZTLOS025)",
                Learner = "John Smith",
                Uln = 1080808080,
                Provider = "Barnsley College (10000536)",
                Tlevel = "Healthcare Science",
                StartYear = "2023 to 2024",
                ExamPeriod = "Summer 2024",
                Grade = string.Empty,
                SelectedGradeId = 10
            };
    }
}