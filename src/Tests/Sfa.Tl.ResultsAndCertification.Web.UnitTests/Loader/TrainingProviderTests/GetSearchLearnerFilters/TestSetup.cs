using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetSearchLearnerFilters
{
    public abstract class TestSetup : TrainingProviderLoaderTestBase
    {
        protected long ProviderUkprn;
        protected SearchLearnerFiltersViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetSearchLearnerFiltersAsync(ProviderUkprn);
        }
    }
}
