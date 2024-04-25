using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminCoreAppealOutcomeReviewChangesPost

{
    public abstract class TestSetup : AdminPostResultsControllerTestBase
    {
        protected const int RegistrationPathwayId = 1, PathwayAssessmentId = 1, PathwayResultId = 146;

        protected static AdminReviewChangesAppealOutcomeCoreViewModel CreateViewModel()
            => new()
            {
                RegistrationPathwayId = RegistrationPathwayId,
                PathwayAssessmentId = PathwayAssessmentId,
                PathwayResultId = PathwayResultId,
                PathwayName = "Healthcare Science (6037083X)",
                Learner = "John Smith",
                Uln = 1080808080,
                Provider = "Barnsley College (10000536)",
                Tlevel = "Healthcare Science",
                StartYear = "2023 to 2024",
                ExamPeriod = "Summer 2024",
                Grade = "A*"
            };
    }
}