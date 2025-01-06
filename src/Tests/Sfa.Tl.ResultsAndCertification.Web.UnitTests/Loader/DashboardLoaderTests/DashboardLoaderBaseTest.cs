using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DashboardLoaderTests
{
    public abstract class DashboardLoaderBaseTest : BaseTest<DashboardLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected DashboardLoader Loader;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            Loader = new DashboardLoader(ApiClient);
        }
    }
}