using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.GetAllProviderTlevelsAsync
{
    public class When_GetAllProviderTlevelsAsync_IsCalled_With_Valid_Ukprn_And_ProviderId : ProviderServiceBaseTest
    {
        private IList<TlRoute> _routes;
        private IList<TlPathway> _pathways;
        private IList<TqAwardingOrganisation> _tqAwardingOrganisations;

        private ProviderTlevels _result;

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
            _result = ProviderService.GetAllProviderTlevelsAsync(TlAwardingOrganisation.UkPrn, TlProvider.Id).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            var actualResult = _result;
            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(TlProvider.Id);
            actualResult.DisplayName.Should().Be(TlProvider.DisplayName);
            actualResult.Ukprn.Should().Be(TlProvider.UkPrn);            
        }

        [Fact]
        public void The_Provider_Tlevels_Is_Not_Null()
        {
            _result.Tlevels.Should().NotBeNull();
        }

        [Fact]
        public void Then_Provider_Tlevels_Count_Is_As_Expected()
        {
            _result.Tlevels.Count().Should().Be(2);
        }

        [Fact]
        public void Then_Provider_Tlevels_Data_As_Expected()
        {
            var firstTlevel = _result.Tlevels.FirstOrDefault();
            firstTlevel.TqAwardingOrganisationId.Should().Be(_tqAwardingOrganisations.First().Id);
            firstTlevel.TqProviderId.Should().Be(TqProvider.Id);  // first tlevel id exists.

            var secondTlevel = _result.Tlevels.Last();
            secondTlevel.TqAwardingOrganisationId.Should().Be(_tqAwardingOrganisations.Last().Id);
            secondTlevel.TqProviderId.Should().Be(null);  // second tlevel id is null (outerjoin)
        }
        protected override void SeedTestData()
        {
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, EnumAwardingOrganisation.Pearson);
            _routes = TlevelDataProvider.CreateTlRoutes(DbContext, EnumAwardingOrganisation.Pearson);
            _pathways = TlevelDataProvider.CreateTlPathways(DbContext, EnumAwardingOrganisation.Pearson, _routes);
            _tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, EnumAwardingOrganisation.Pearson, TlAwardingOrganisation, _pathways);
            SeedProviderData();
            DbContext.SaveChangesAsync();
        }

        protected override void SeedProviderData()
        {
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, _tqAwardingOrganisations.First(), TlProvider);
        }
    }
}
