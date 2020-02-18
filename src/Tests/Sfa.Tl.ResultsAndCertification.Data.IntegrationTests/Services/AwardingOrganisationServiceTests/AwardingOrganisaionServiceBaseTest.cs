using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AwardingOrganisationServiceTests
{
    public abstract class AwardingOrganisaionServiceBaseTest : BaseTest<TqAwardingOrganisation>
    {
        protected IMapper _mapper;
        protected ILogger<IRepository<TqAwardingOrganisation>> _logger;        
        protected AwardingOrganisationService _service;
        protected TlRoute _route;
        protected TlPathway _pathway;        
        protected TqAwardingOrganisation _tqAwardingOrganisation;
        protected IEnumerable<AwardingOrganisationPathwayStatus> _result;

        public void CreateMapper()
        {
            _mapper = new MapperConfiguration(cfg =>
                                cfg.CreateMap<TqAwardingOrganisation, AwardingOrganisationPathwayStatus>()
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.TlPathway.Id))
                .ForMember(d => d.RouteName, opts => opts.MapFrom(s => s.TlRoute.Name))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.TlPathway.Name))
                .ForMember(d => d.StatusId, opts => opts.MapFrom(s => s.ReviewStatus)))
                .CreateMapper();
        }

        protected virtual void SeedTlevelTestData()
        {
            TlevelDataProvider.CreateTlAwardingOrganisation(DbContext);
            _route = TlevelDataProvider.CreateTlRoute(DbContext);
            _pathway = TlevelDataProvider.CreateTlPathway(DbContext);
            _tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext);
            DbContext.SaveChangesAsync();
        }
    }
}
