using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.GetSoaLearnerRecordDetails
{
    public abstract class TestSetup : StatementOfAchievementLoaderTestBase
    {
        protected long ProviderUkprn;
        protected int ProfileId;
        protected SoaLearnerRecordDetailsViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetSoaLearnerRecordDetailsAsync(ProviderUkprn, ProfileId);
        }
    }
}
