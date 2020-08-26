using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests
{
    public abstract class AwardingOrganisaionServiceBaseTest : BaseTest<TqAwardingOrganisation>
    {
        protected IMapper _mapper;
        protected ILogger<IRepository<TqAwardingOrganisation>> _logger;        
        protected AwardingOrganisationService _service;
        protected TlRoute _route;
        protected TlPathway _pathway;
        protected TlAwardingOrganisation _tlAwardingOrganisation;
        protected TqAwardingOrganisation _tqAwardingOrganisation;
        protected IEnumerable<AwardingOrganisationPathwayStatus> _result;
        protected ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AwardingOrganisationMapper).Assembly));
            _mapper = new Mapper(mapperConfig);
        }

        protected virtual void SeedTlevelTestData()
        {
            _tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, EnumAwardingOrganisation.Ncfe);
            _route = TlevelDataProvider.CreateTlRoute(DbContext, EnumAwardingOrganisation.Ncfe);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext, EnumAwardingOrganisation.Ncfe);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, EnumAwardingOrganisation.Ncfe);
            DbContext.SaveChangesAsync();
        }
    }
}
