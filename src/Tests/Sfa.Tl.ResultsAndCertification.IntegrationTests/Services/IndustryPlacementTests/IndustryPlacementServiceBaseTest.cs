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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementTests
{
    public abstract class IndustryPlacementServiceBaseTest : BaseTest<TqRegistrationProfile>
    {
        protected int? PathwayId;

        // Data Seed variables
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TqProvider TqProvider;
        protected IEnumerable<TlProvider> TlProviders;
        protected IList<TqProvider> TqProviders;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<Qualification> Qualifications;

        protected IndustryPlacementService IndustryPlacementService;
        protected IRepository<IpLookup> IpLookupRepository;
        protected IRepository<IpModelTlevelCombination> IpModelTlevelCombinationRepository;
        protected IRepository<IpTempFlexTlevelCombination> IpTempFlexTlevelCombinationRepository;
        protected IRepository<IpTempFlexNavigation> IpTempFlexNavigationRepository;
        protected IRepository<IndustryPlacement> IndustryPlacementRepository;        
        protected IRepository<TqRegistrationPathway> RegistrationPathwayRepository;

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
        protected ILogger<GenericRepository<TqRegistrationPathway>> RegistrationPathwayRepositoryLogger;
        protected ILogger<IndustryPlacementService> IndustryPlacementServiceLogger;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(IndustryPlacementMapper).Assembly));
            Mapper = new Mapper(mapperConfig);
        }

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialism = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway).First();
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProviders.First());
            //TlLookup = TlLookupDataProvider.CreateIpLookupList(DbContext, null, true);

            DbContext.SaveChanges();
        }
        public List<TqRegistrationProfile> SeedRegistrationsData(Dictionary<long, RegistrationPathwayStatus> ulns, TqProvider tqProvider = null)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedRegistrationData(uln.Key, uln.Value, tqProvider));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildListWithoutLrsData().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            if (status == RegistrationPathwayStatus.Withdrawn || status == RegistrationPathwayStatus.Transferred)
            {
                tqRegistrationPathway.Status = status;
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                tqRegistrationSpecialism.IsOptedin = true;
                tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
            }

            DbContext.SaveChanges();
            return profile;
        }


        public void SeedIpLookupData()
        {
            IpLookup = IpLookupDataProvider.CreateIpLookupList(DbContext, null, IpLookupType.IndustryPlacementModel, true);
            DbContext.SaveChanges();
        }

        public void SeedSpecialConsiderationsLookupData()
        {
            IpLookup = IpLookupDataProvider.CreateIpLookupList(DbContext, null, IpLookupType.SpecialConsideration, true);
            DbContext.SaveChanges();
        }

        public void SeedTempFlexNavigation()
        {
            var navigations = new IpTempFlexNavigationBuilder().BuildList(EnumAwardingOrganisation.Pearson);
            DbContext.AddRange(navigations);
            DbContext.SaveChanges();
        }

        public void SeedIpModelTlevelCombinationsData(TlPathway pathway = null)
        {
            var ipModelTlevelCombinations = new IpModelTlevelCombinationBuilder().BuildList(EnumAwardingOrganisation.Pearson, IpLookupType.IndustryPlacementModel, pathway);

            IpModelTlevelCombination = IpModelTlevelCombinationProvider.CreateIpModelTlevelCombinationsList(DbContext, EnumAwardingOrganisation.Ncfe, ipModelTlevelCombinations, true);

            DbContext.SaveChanges();
        }

        public void SeedIpTempFlexTlevelCombinationsData(TlPathway pathway = null)
        {
            var ipTempFlexTlevelCombinations = new IpTempFlexTlevelCombinationBuilder().BuildList(EnumAwardingOrganisation.Pearson, IpLookupType.TemporaryFlexibility, pathway);

            IpTempFlexTlevelCombination = IpTempFlexTlevelCombinationProvider.CreateIpTempFlexTlevelCombinationsList(DbContext, EnumAwardingOrganisation.Pearson, ipTempFlexTlevelCombinations, true);

            DbContext.SaveChanges();
        }
    }

    public enum Provider
    {
        BarsleyCollege = 10000536,
        WalsallCollege = 10007315
    }
}