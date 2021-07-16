using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.FindPrsLearnerRecord
{
    public abstract class TestSetup : PostResultsServiceLoaderTestBase
    {
        protected long AoUkprn;
        protected long? Uln;
        protected int? ProfileId;
        protected Models.Contracts.PostResultsService.FindPrsLearnerRecord ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.FindPrsLearnerRecordAsync(AoUkprn, Uln, ProfileId);
        }
    }
}
