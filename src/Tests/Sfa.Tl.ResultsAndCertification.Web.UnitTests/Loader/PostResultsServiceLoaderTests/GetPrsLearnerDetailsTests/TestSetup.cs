using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.GetPrsLearnerDetailsTests
{
    public abstract class TestSetup : PostResultsServiceLoaderTestBase
    {
        protected long AoUkprn;
        protected int ProfileId;
        protected int AssessmentId;

        protected PrsLearnerDetailsViewModel ActualResult { get; set; }
    }
}
