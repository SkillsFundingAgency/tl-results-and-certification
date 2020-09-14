using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.FindProvider
{
    public class When_Called_With_Part_Match : ProviderServiceBaseTest
    {
        private IEnumerable<ProviderMetadata> _result;
        
        // parameters
        private string _providerName = "Wal";
        private bool _isExactMatch = false;

        public override void Given()
        {
            SeedProviders();
            CreateMapper();
            ProviderRepositoryLogger = Substitute.For<ILogger<ProviderRepository>>();
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            TlProviderRepositoryLogger = Substitute.For<ILogger<GenericRepository<TlProvider>>>();
            TlproviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);
            ProviderService = new ProviderService(ProviderRepository, TlproviderRepository, ProviderMapper, Logger);
        }

        private void SeedProviders()
        {
            ProviderDataProvider.CreateTlProviders(DbContext);
            DbContext.SaveChangesAsync();
        }

        public async override Task When()
        {
            _result = await ProviderService.FindProviderAsync(_providerName, _isExactMatch);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Count().Should().Be(2);
            _result.First().DisplayName.Should().StartWith(_providerName);
        }
    }
}