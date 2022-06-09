using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SubmitSearchLearnerApplyFilters
{
    public abstract class TestSetup : TrainingProviderControllerTestBase
    {
        public IActionResult Result { get; private set; }
        public SearchCriteriaViewModel SearchCriteriaViewModel;

        public async override Task When()
        {
            Result = await Controller.SubmitSearchLearnerApplyFiltersAsync(SearchCriteriaViewModel);
        }
    }
}
