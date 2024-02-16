using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.RemoveAssessmentEntrySpecialismGet
{
    public abstract class TestSetup : AdminDashboardControllerTestBase
    {
        protected const int RegistrationPathwayId = 1, AssessmentId = 1;

        protected IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.RemoveAssessmentEntrySpecialismAsync(RegistrationPathwayId, AssessmentId);
        }

        protected static AdminRemoveSpecialismAssessmentEntryViewModel ViewModel
            => new()
            {
                RegistrationPathwayId = RegistrationPathwayId,
                SpecialismAssessmentId = AssessmentId,
                SpecialismName = "Assisting with Healthcare Science (ZTLOS018)",
                Learner = "John Smith",
                Uln = 1080808080,
                Provider = "Barnsley College (10000536)",
                Tlevel = "Healthcare Science",
                StartYear = "2023 to 2024",
                ExamPeriod = "Summer 2024",
                Grade = string.Empty,
                LastUpdated = "12/06/2023",
                UpdatedBy = "test-user",
                CanAssessmentEntryBeRemoved = true,
                DoYouWantToRemoveThisAssessmentEntry = true
            };
    }
}