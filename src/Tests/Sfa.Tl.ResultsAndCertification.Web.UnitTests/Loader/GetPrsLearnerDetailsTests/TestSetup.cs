using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.GetPrsLearnerDetailsTests
{
    public abstract class TestSetup : PostResultsServiceLoaderTestBase
    {
        protected long AoUkprn;
        protected int ProfileId;
        protected int AssessmentId;

        protected PrsLearnerDetailsViewModel ActualResult { get; set; }

        public async override Task When()
        {
            ActualResult = await Loader.GetPrsLearnerDetailsAsync<PrsLearnerDetailsViewModel>(AoUkprn, ProfileId, AssessmentId);
        }
    }
}
