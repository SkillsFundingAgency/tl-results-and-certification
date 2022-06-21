using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerClearFilters
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public int AcademicYear { get; set; }
        public IActionResult Result { get; private set; }

        public async override Task When()
        {
            Result = await Controller.SearchLearnerClearFiltersAsync(AcademicYear);
        }
    }
}