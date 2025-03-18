using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Loader;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminAwardingOrganisationLoaderTests
{
    public abstract class AdminAwardingOrganisationLoaderBaseTest : BaseTest<AdminAwardingOrganisationLoader>
    {
        protected IResultsAndCertificationInternalApiClient ApiClient;
        protected AdminAwardingOrganisationLoader Loader;

        public override void Setup()
        {
            ApiClient = Substitute.For<IResultsAndCertificationInternalApiClient>();
            Loader = new AdminAwardingOrganisationLoader(ApiClient);
        }
    }
}