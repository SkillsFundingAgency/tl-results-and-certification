using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.StatementOfAchievementLoaderTests.CreateSoaPrintingRequest
{
    public abstract class TestSetup : StatementOfAchievementLoaderTestBase
    {
        protected long ProviderUkprn;
        protected SoaLearnerRecordDetailsViewModel SoaLearnerRecordDetailsViewModel { get; set; }
        protected SoaPrintingResponse ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.CreateSoaPrintingRequestAsync(ProviderUkprn, SoaLearnerRecordDetailsViewModel);
        }
    }
}
