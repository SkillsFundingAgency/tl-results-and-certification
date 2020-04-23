using System.Linq;
using Xunit;
using FluentAssertions;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.GetAllProviderTlevelsAsync
{
    public class When_GetAllProviderTlevelsAsync_IsCalled_With_Returns_In_Correct_Order : ProviderServiceBaseTest
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
        public void Then_Expected_Tlevels_Results_Is_Returned_In_Ascending_Order()
        {
            var actualResult = _result;
            actualResult.Should().NotBeNull();
            actualResult.Id.Should().Be(TlProvider.Id);
            actualResult.DisplayName.Should().Be(TlProvider.DisplayName);
            actualResult.Ukprn.Should().Be(TlProvider.UkPrn);

            actualResult.Tlevels.Should().BeInAscendingOrder(x => x.TlevelTitle);
            actualResult.Tlevels.Count().Should().Be(2);
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
