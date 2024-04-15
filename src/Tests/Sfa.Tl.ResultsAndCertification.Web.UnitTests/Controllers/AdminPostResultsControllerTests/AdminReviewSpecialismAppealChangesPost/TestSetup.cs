using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminReviewSpecialismAppealChangesPost
{
    public abstract class TestSetup : AdminPostResultsControllerTestBase
    {
        protected const int RegistrationPathwayId = 1, SpecialismAssessmentId = 1, SpecialismResultId = 146;

        protected static AdminAppealSpecialismReviewChangesViewModel CreateViewModel()
            => new()
            {
                RegistrationPathwayId = RegistrationPathwayId,
                SpecialismAssessmentId = SpecialismAssessmentId,
                SpecialismResultId = SpecialismResultId,
                SpecialismName = "Assisting with Healthcare Science (ZTLOS018)",
                Learner = "John Smith",
                Uln = 1080808080,
                Provider = "Barnsley College (10000536)",
                Tlevel = "Healthcare Science",
                StartYear = "2023 to 2024",
                ExamPeriod = "Summer 2024",
                Grade = "Merit"
            };
    }
}