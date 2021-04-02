using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.ProcessIndustryPlacementQuestionUpdate
{
    public abstract class TestSetup : TrainingProviderLoaderTestBase
    {
        protected long ProviderUkprn;
        protected int ProfileId;
        protected UpdateIndustryPlacementQuestionViewModel ViewModel;
        protected UpdateLearnerRecordResponse ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.ProcessIndustryPlacementQuestionUpdateAsync(ProviderUkprn, ViewModel);
        }
    }
}
