using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.SearchLearnerDetails
{
    public abstract class TestSetup : TrainingProviderLoaderTestBase
    {
        protected long ProviderUkprn;
        protected int AcademicYear;
        protected int? PageNumber;
        protected SearchCriteriaViewModel SearchCriteriaViewModel;
        protected SearchLearnerDetailsListViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.SearchLearnerDetailsAsync(ProviderUkprn, SearchCriteriaViewModel);
        }
    }
}
