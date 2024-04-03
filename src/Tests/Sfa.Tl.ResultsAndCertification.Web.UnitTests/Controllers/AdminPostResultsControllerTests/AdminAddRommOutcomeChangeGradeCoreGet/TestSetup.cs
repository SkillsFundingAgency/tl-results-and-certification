using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddRommOutcomeChangeGradeCoreGet

{
    public abstract class TestSetup : AdminPostResultsControllerTestBase
    {
        protected const int RegistrationPathwayId = 1, AssessmentId = 1;

        protected IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.AdminAddRommOutcomeChangeGradeCoreAsync(RegistrationPathwayId, AssessmentId);
        }

        protected static AdminAddRommOutcomeChangeGradeCoreViewModel ViewModel
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
                SelectedGradeId = 1
            };
    }
}