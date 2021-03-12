using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.TrainingProviderTests.FindLearnerRecord
{
    public abstract class TestSetup : TrainingProviderLoaderTestBase
    {
        protected long ProviderUkprn;
        protected long Uln;
        protected Models.Contracts.TrainingProvider.FindLearnerRecord ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.FindLearnerRecordAsync(ProviderUkprn, Uln);
        }
    }
}
