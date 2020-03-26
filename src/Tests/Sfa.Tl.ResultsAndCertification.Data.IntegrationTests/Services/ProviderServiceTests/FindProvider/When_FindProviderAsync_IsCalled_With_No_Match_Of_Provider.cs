using Xunit;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System.Linq;
using FluentAssertions;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.FindProvider
{
    public class When_FindProviderAsync_IsCalled_With_No_Match_Of_Provider : ProviderServiceBaseTest
    {
        private IEnumerable<ProviderMetadata> _result;
        
        // parameters
        private string _providerName = "Test Provider";
        private bool _isExactMatch = true;

        public override void Given()
        {
            SeedProviders();
            
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

        public override void When()
        {
            _result = ProviderService.FindProviderAsync(_providerName, _isExactMatch).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Count().Should().Be(0);
        }
    }
}