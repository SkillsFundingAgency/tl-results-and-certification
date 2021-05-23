using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.FindSoaLearnerRecord
{
    public abstract class TestSetup : StatementOfAchievementLoaderTestBase
    {
        protected long ProviderUkprn;
        protected long Uln;
        protected Models.Contracts.StatementOfAchievement.FindSoaLearnerRecord ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.FindSoaLearnerRecordAsync(ProviderUkprn, Uln);
        }
    }
}
