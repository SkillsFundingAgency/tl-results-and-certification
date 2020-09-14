using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.GetSelectProviderTlevel
{
    public class When_Called_With_Invalid_Ukprn_And_ProviderId : ProviderServiceBaseTest
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

        public async override Task When()
        {
            _result = await ProviderService.GetSelectProviderTlevelsAsync(_invalidUkprn, _invalidProviderId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeNull();
        }
    }
}
