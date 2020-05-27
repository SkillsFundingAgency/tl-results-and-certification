using Xunit;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.HasAnyTlevelSetupForProviderAsync
{
    public class When_HasAnyTlevelSetupForProviderAsync_Is_Called_With_Valid_TlProviderId : ProviderServiceBaseTest
    {
        private bool _result;

        public override void Given()
        {
            SeedTestData();

            ProviderRepositoryLogger = Substitute.For<ILogger<ProviderRepository>>();
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            TlProviderRepositoryLogger = Substitute.For<ILogger<GenericRepository<TlProvider>>>();
            TlproviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);
            ProviderService = new ProviderService(ProviderRepository, TlproviderRepository, ProviderMapper, Logger);
        }

        protected override void SeedProviderData()
        {
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProvider);
        }

        public override void When()
        {
            _result = ProviderService.HasAnyTlevelSetupForProviderAsync(TlAwardingOrganisation.UkPrn, TlProvider.Id).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            _result.Should().BeTrue();
        }
    }
}
