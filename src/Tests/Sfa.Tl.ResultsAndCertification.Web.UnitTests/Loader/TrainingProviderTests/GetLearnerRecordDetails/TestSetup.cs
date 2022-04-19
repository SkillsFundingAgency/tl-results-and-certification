using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.GetLearnerRecordDetails
{
    public abstract class TestSetup : TrainingProviderLoaderTestBase
    {
        protected long ProviderUkprn;
        protected int ProfileId;
        protected LearnerRecordDetailsViewModel1 ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetLearnerRecordDetailsAsync<LearnerRecordDetailsViewModel1>(ProviderUkprn, ProfileId);
        }        
    }
}