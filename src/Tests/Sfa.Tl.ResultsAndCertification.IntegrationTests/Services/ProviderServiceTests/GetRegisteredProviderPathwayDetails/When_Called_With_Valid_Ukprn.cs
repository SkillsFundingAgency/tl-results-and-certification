using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.GetRegisteredProviderPathwayDetails
{
    public class When_Called_With_Valid_Ukprn : ProviderServiceBaseTest
    {
        private IList<TlRoute> _routes;
        private IList<TlPathway> _pathways;
        private IList<TqAwardingOrganisation> _tqAwardingOrganisations;
        private IList<PathwayDetails> _result;

        public override void Given()
        {
            SeedTestData();
            CreateMapper();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            TlProviderRepositoryLogger = new Logger<GenericRepository<TlProvider>>(new NullLoggerFactory());
            TlproviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);
            ProviderService = new ProviderService(ProviderRepository, TlproviderRepository, ProviderMapper, Logger);
        }

        public async override Task When()
        {
            _result = await ProviderService.GetRegisteredProviderPathwayDetailsAsync(TlAwardingOrganisation.UkPrn, TlProvider.UkPrn);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Count().Should().Be(1);

            var actualResult = _result.FirstOrDefault();
            actualResult.Id.Should().Be(Pathway.Id);
            actualResult.Code.Should().Be(Pathway.LarId);
            actualResult.Name.Should().Be(Pathway.Name);
        }

        protected override void SeedTestData()
        {
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, EnumAwardingOrganisation.Pearson);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, EnumAwardingOrganisation.Pearson);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, EnumAwardingOrganisation.Pearson, Route);
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProvider);
            DbContext.SaveChangesAsync();
        }
    }
}
