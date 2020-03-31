using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ProviderServiceTests.AddProviderTlevelsAsync
{
    public class When_AddProviderTlevelsAsync_IsCalled_Returns_Success : ProviderServiceBaseTest
    {
        private bool _isSuccess;
        private IList<TlRoute> _routes;
        private IList<TlPathway> _pathways;
        private List<ProviderTlevelDetails> _providerTlevelDetails;
        private IList<TqAwardingOrganisation> _tqAwardingOrganisations;
        private readonly EnumAwardingOrganisation _awardingOrganisation = EnumAwardingOrganisation.Pearson;

        public override void Given()
        {
            SeedTestData();
            CreateMapper();
            ProviderRepositoryLogger = Substitute.For<ILogger<ProviderRepository>>();
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            TlProviderRepositoryLogger = Substitute.For<ILogger<GenericRepository<TlProvider>>>();
            TlproviderRepository = new GenericRepository<TlProvider>(TlProviderRepositoryLogger, DbContext);
            ProviderService = new ProviderService(ProviderRepository, TlproviderRepository, ProviderMapper, Logger);

            _providerTlevelDetails = new List<ProviderTlevelDetails>();

            foreach(var tqAo in _tqAwardingOrganisations)
            {
                _providerTlevelDetails.Add(new ProviderTlevelDetails { TqAwardingOrganisationId = tqAo.Id, ProviderId = TlProvider.Id, PathwayId = tqAo.TlPathwayId, CreatedBy = "test user" });
            }
        }

        public override void When()
        {
            _isSuccess = ProviderService.AddProviderTlevelsAsync(_providerTlevelDetails).Result;
        }

        [Fact]
        public void Then_Record_Is_Saved()
        {
            _isSuccess.Should().BeTrue();
        }

        protected override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("DateTimeResolver") ?
                                new DateTimeResolver<ProviderTlevelDetails, TqProvider>(new DateTimeProvider()) :
                                null);
            });
            ProviderMapper = new Mapper(mapperConfig);
        }

        protected override void SeedTestData()
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, _awardingOrganisation);
            _routes = TlevelDataProvider.CreateTlRoutes(DbContext, _awardingOrganisation);
            _pathways = TlevelDataProvider.CreateTlPathways(DbContext, _awardingOrganisation, _routes);
            _tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, _awardingOrganisation, TlAwardingOrganisation, _pathways);
            SeedProviderData();
            DbContext.SaveChangesAsync();
            DetachEntity<TqAwardingOrganisation>();
        }
    }
}
