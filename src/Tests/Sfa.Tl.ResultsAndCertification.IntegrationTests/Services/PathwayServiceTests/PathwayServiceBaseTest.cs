using System.Collections.Generic;
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
        protected IList<TlSpecialism> _specialisms;
        protected TlAwardingOrganisation _tlAwardingOrganisation;
        protected TqAwardingOrganisation _tqAwardingOrganisation;
        protected TlevelPathwayDetails _result;

        public void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(PathwayMapper).Assembly));
            _mapper = new Mapper(mapperConfig);            
        }

        protected virtual void SeedTlevelTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Ncfe)
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, _route);
            _specialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, _pathway);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, _pathway, _tlAwardingOrganisation);
            DbContext.SaveChangesAsync();
        }
    }
}
