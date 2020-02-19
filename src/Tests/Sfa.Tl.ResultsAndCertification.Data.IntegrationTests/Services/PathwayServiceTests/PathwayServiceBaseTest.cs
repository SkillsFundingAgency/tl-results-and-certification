using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PathwayServiceTests
{
    public abstract class PathwayServiceBaseTest : BaseTest<TlPathway>
    {
        protected IMapper _mapper;
        protected ILogger<IRepository<TlPathway>> _logger;
        protected PathwayService _service;
        protected TlRoute _route;
        protected TlPathway _pathway;
        protected TlAwardingOrganisation _tlAwardingOrganisation;
        protected TqAwardingOrganisation _tqAwardingOrganisation;
        protected TlevelPathwayDetails _result;

        public void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(PathwayMappingProfile).Assembly));
            _mapper = new Mapper(mapperConfig);            
        }

        protected virtual void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, EnumAwardingOrganisation.Ncfe);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, EnumAwardingOrganisation.Ncfe);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, EnumAwardingOrganisation.Ncfe, _route);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, _route, _pathway, _tlAwardingOrganisation);
            DbContext.SaveChangesAsync();
        }
    }
}
