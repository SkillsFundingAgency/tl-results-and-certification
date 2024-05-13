using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.SearchRegistrationServiceTests
{
    public abstract class SearchRegistrationServiceBaseTest : BaseTest<SearchRegistrationService>
    {
        protected ISearchRegistrationRepository SearchRegistrationRepository;
        protected SearchRegistrationService RegistrationService;

        public override void Setup()
        {
            SearchRegistrationRepository = Substitute.For<ISearchRegistrationRepository>();
            RegistrationService = new SearchRegistrationService(SearchRegistrationRepository);
        }
    }
}