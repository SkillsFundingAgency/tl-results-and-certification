﻿using FluentAssertions;
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
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.GetAllProviderTlevels
{
    public class When_Called_With_InValid_Ukprn_And_ProviderId : ProviderServiceBaseTest
    {
        private IList<TlRoute> _routes;
        private IList<TlPathway> _pathways;
        private IList<TqAwardingOrganisation> _tqAwardingOrganisations;

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
            _result = await ProviderService.GetAllProviderTlevelsAsync(_invalidUkprn, _invalidProviderId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeNull();
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
