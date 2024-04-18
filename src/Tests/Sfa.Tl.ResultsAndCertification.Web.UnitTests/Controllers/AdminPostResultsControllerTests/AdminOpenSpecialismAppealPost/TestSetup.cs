using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenSpecialismAppealPost
{
    public abstract class TestSetup : AdminPostResultsControllerTestBase
    {
        protected const int RegistrationPathwayId = 1, AssessmentId = 1, SpecialismResultId = 1876;
        protected IActionResult Result;

        protected static AdminOpenSpecialismAppealViewModel CreateViewModel(bool? doYouWantToOpenAppeal = null)
            => new()
            {
                RegistrationPathwayId = RegistrationPathwayId,
                SpecialismAssessmentId = AssessmentId,
                SpecialismResultId = SpecialismResultId,
                SpecialismName = "Assisting with Healthcare Science (ZTLOS018)",
                Learner = "John Smith",
                Uln = 1080808080,
                Provider = "Barnsley College (10000536)",
                Tlevel = "Healthcare Science",
                StartYear = "2023 to 2024",
                ExamPeriod = "Summer 2024",
                Grade = string.Empty,
                DoYouWantToOpenAppeal = doYouWantToOpenAppeal
            };
    }
}