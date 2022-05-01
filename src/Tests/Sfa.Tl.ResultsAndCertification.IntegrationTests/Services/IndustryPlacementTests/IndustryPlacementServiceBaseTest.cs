using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests
{
    public abstract class IndustryPlacementServiceBaseTest : BaseTest<TqRegistrationProfile>
    {
        protected IndustryPlacementService IndustryPlacementService;
        protected IRepository<IpLookup> IpLookupRepository;
        protected IRepository<IpModelTlevelCombination> IpModelTlevelCombinationRepository;

        protected IMapper Mapper;
        protected IList<IpLookup> IpLookup;

        // Dependencies
        protected ILogger<GenericRepository<IpLookup>> IpLookupRepositoryLogger;
        protected ILogger<GenericRepository<IpModelTlevelCombination>> IpModelTlevelCombinationLogger;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(IndustryPlacementMapper).Assembly));
            Mapper = new Mapper(mapperConfig);
        }

        public void SeedIpLookupData()
        {
            IpLookup = IpLookupDataProvider.CreateIpLookupList(DbContext, null, IpLookupType.SpecialConsideration, true);
            DbContext.SaveChangesAsync();
        }
    }
}
