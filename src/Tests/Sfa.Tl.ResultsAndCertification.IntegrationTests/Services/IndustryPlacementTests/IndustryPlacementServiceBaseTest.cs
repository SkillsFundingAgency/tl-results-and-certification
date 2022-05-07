using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests
{
    public abstract class IndustryPlacementServiceBaseTest : BaseTest<TqRegistrationProfile>
    {
        protected int? PathwayId;

        protected IndustryPlacementService IndustryPlacementService;
        protected IRepository<IpLookup> IpLookupRepository;
        protected IRepository<IpModelTlevelCombination> IpModelTlevelCombinationRepository;
        protected IRepository<IpTempFlexTlevelCombination> IpTempFlexTlevelCombinationRepository;
        protected IRepository<IpTempFlexNavigation> IpTempFlexNavigationRepository;
        protected IRepository<IndustryPlacement> IndustryPlacementRepository;

        protected IMapper Mapper;
        protected IList<IpLookup> IpLookup;
        protected IList<IpModelTlevelCombination> IpModelTlevelCombination;
        protected IList<IpTempFlexTlevelCombination> IpTempFlexTlevelCombination;

        // Dependencies
        protected ILogger<GenericRepository<IpLookup>> IpLookupRepositoryLogger;
        protected ILogger<GenericRepository<IpModelTlevelCombination>> IpModelTlevelCombinationLogger;
        protected ILogger<GenericRepository<IpTempFlexTlevelCombination>> IpTempFlexTlevelCombinationLogger;
        protected ILogger<GenericRepository<IpTempFlexNavigation>> IpTempFlexNavigationLogger;
        protected ILogger<GenericRepository<IndustryPlacement>> IndustryPlacementLogger;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(IndustryPlacementMapper).Assembly));
            Mapper = new Mapper(mapperConfig);
        }

        public void SeedIpLookupData()
        {
            IpLookup = IpLookupDataProvider.CreateIpLookupList(DbContext, null, IpLookupType.IndustryPlacementModel, true);
            DbContext.SaveChangesAsync();
        }

        public void SeedTempFlexNavigation()
        {
            var navigations = new IpTempFlexNavigationBuilder().BuildList(EnumAwardingOrganisation.Pearson);
            DbContext.AddRange(navigations);
            DbContext.SaveChanges();
        }

        public void SeedIpModelTlevelCombinationsData()
        {
            var ipModelTlevelCombinations = new IpModelTlevelCombinationBuilder().BuildList(EnumAwardingOrganisation.Ncfe, IpLookupType.IndustryPlacementModel);

            IpModelTlevelCombination = IpModelTlevelCombinationProvider.CreateIpModelTlevelCombinationsList(DbContext, EnumAwardingOrganisation.Ncfe, ipModelTlevelCombinations, true);

            DbContext.SaveChangesAsync();
        }

        public void SeedIpTempFlexTlevelCombinationsData()
        {
            var ipTempFlexTlevelCombinations = new IpTempFlexTlevelCombinationBuilder().BuildList(EnumAwardingOrganisation.Ncfe, IpLookupType.TemporaryFlexibility);

            IpTempFlexTlevelCombination = IpTempFlexTlevelCombinationProvider.CreateIpTempFlexTlevelCombinationsList(DbContext, EnumAwardingOrganisation.Ncfe, ipTempFlexTlevelCombinations, true);

            DbContext.SaveChangesAsync();
        }
    }
}