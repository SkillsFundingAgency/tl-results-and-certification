using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDownloadLearnerResultsLoaderTests
{
    public abstract class AdminDownloadLearnerResultsLoaderBaseTest : BaseTest<AdminDownloadLearnerResultsLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected AdminDownloadLearnerResultsLoader Loader;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            Loader = new AdminDownloadLearnerResultsLoader(ApiClient);
        }
    }
}