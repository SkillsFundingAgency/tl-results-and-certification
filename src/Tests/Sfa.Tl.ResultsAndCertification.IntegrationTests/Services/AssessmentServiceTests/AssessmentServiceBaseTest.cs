using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    public abstract class AssessmentServiceBaseTest : BaseTest<TqPathwayAssessment>
    {
        protected AssessmentService AssessmentService;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected IList<TlSpecialism> Specialisms;
        protected TlProvider TlProvider;
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TqProvider TqProvider;
        protected IList<TlProvider> TlProviders;
        protected IList<TqProvider> TqProviders;
        protected IList<AssessmentSeries> AssessmentSeries;        
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected IAssessmentRepository AssessmentRepository;
        protected ILogger<AssessmentRepository> AssessmentRepositoryLogger;
        protected IRepository<AssessmentSeries> AssessmentSeriesRepository;
        protected ILogger<GenericRepository<AssessmentSeries>> AssessmentSeriesRepositoryLogger;

        protected IMapper AssessmentMapper;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(RegistrationMapper).Assembly));
            AssessmentMapper = new Mapper(mapperConfig);
        }

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialism = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway).First();
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProvider);
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            DbContext.SaveChangesAsync();
        }
        //public List<TqRegistrationProfile> SeedRegistrationsData1(List<long> ulns, TqProvider tqProvider = null)
        //{
        //    var profiles = new List<TqRegistrationProfile>();

        //    foreach (var uln in ulns)
        //    {
        //        profiles.Add(SeedRegistrationData(uln, tqProvider));
        //    }
        //    return profiles;
        //}

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
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            tqRegistrationPathway.Status = status;
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            DbContext.SaveChanges();
            return profile;
        }
    }
}
