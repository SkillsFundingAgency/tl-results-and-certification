using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.GetSelectProviderTlevelAsync
{
    public class When_GetSelectProviderTlevelsAsync_IsCalled_With_InValid_Ukprn_And_InValid_ProviderId : ProviderServiceBaseTest
    {
        private ProviderTlevels _result;
        private int _invalidUkprn = 3000;
        private int _invalidProviderId = 89000;
        public override void Given()
        {
            SeedTestData();

            ProviderRepositoryLogger = Substitute.For<ILogger<ProviderRepository>>();
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            TlProviderRepositoryLogger = Substitute.For<ILogger<GenericRepository<TlProvider>>>();
            TlproviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);
            ProviderService = new ProviderService(ProviderRepository, TlproviderRepository, ProviderMapper, Logger);
        }

        public override void When()
        {
            _result = ProviderService.GetSelectProviderTlevelsAsync(_invalidUkprn, _invalidProviderId).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            _result.Should().BeNull();
        }
    }
}
