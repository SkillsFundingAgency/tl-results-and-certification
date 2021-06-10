using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.GetPrintRequestSnapshot
{
    public abstract class TestSetup : StatementOfAchievementLoaderTestBase
    {
        protected long ProviderUkprn;
        protected int ProfileId;
        protected int PathwayId;
        protected RequestSoaAlreadySubmittedViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrintRequestSnapshotAsync(ProviderUkprn, ProfileId, PathwayId);
        }
    }
}
