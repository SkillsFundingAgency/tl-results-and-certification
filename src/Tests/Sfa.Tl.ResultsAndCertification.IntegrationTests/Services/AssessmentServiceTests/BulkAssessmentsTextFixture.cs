using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Respawn;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    public class BulkAssessmentsTextFixture : IDisposable
    {
        //public long Uln;
        public List<long> Ulns;
        protected IList<TlProvider> TlProviders;
        public IList<TqProvider> TqProviders;
        protected IList<TlSpecialism> TlSpecialisms;
        protected AssessmentService AssessmentService;
        protected ILogger<AssessmentRepository> AssessmentRepositoryLogger;
        protected ILogger<GenericRepository<AssessmentSeries>> AssessmentSeriesRepositoryLogger;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        public IAssessmentRepository AssessmentRepository;
        protected IRepository<TqPathwayAssessment> PathwayAssessmentRepository;
        protected IRepository<TqSpecialismAssessment> SpecialismAssessmentRepository;
        protected IRepository<AssessmentSeries> AssessmentSeriesRepository;
        protected IMapper AssessmentMapper;
        public AssessmentProcessResponse Result;
        public IList<TqRegistrationProfile> TqRegistrationProfilesData;
        public IList<TqPathwayAssessment> TqPathwayAssessmentsData;
        public IList<TqSpecialismAssessment> TqSpecialismAssessmentsData;
        protected IList<AssessmentSeries> AssessmentSeries;
        public Checkpoint DbCheckpoint;
        public ResultsAndCertificationDbContext DbContext;

        public BulkAssessmentsTextFixture()
        {
            DbCheckpoint = new Checkpoint { WithReseed = true };
            DbContext = TestDatabaseConfiguration.CreateRelationalDbContext();
            Initialize();
        }

        public void Initialize()
        {
            CreateMapper();
            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);
            AssessmentService = new AssessmentService(AssessmentRepository, PathwayAssessmentRepository, SpecialismAssessmentRepository, AssessmentSeriesRepository, AssessmentMapper, AssessmentRepositoryLogger);
        }

        protected void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(RegistrationMapper).Assembly));
            AssessmentMapper = new Mapper(mapperConfig);
        }

        public async Task WhenAsync()
        {
            Result = await AssessmentService.CompareAndProcessAssessmentsAsync(TqPathwayAssessmentsData, TqSpecialismAssessmentsData);
        }

        public void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson)
        {
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext).ToList();
            var tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            var routes = TlevelDataProvider.CreateTlRoutes(DbContext, awardingOrganisation);
            var pathways = TlevelDataProvider.CreateTlPathways(DbContext, awardingOrganisation, routes);
            TlSpecialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, pathways.First());

            var tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, awardingOrganisation, tlAwardingOrganisation, pathways);
            DbContext.SaveChanges();

            TqProviders = new List<TqProvider>();
            foreach (var tqAwardingOrganisation in tqAwardingOrganisations)
            {
                foreach (var tlProvider in TlProviders)
                {
                    TqProviders.Add(ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, tlProvider));
                }
            }

            DbContext.SaveChanges();
        }

        public List<TqRegistrationProfile> SeedRegistrationsData(List<long> ulns, TqProvider tqProvider = null)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach(var uln in ulns)
            {
                profiles.Add(SeedRegistrationData(uln, tqProvider));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, TlSpecialisms.First());

            DbContext.SaveChanges();
            return profile;
        }

        public TqPathwayAssessment SeedPathwayAssessmentData(TqRegistrationProfile registrationProfile)
        {
            var pathwayAssessment = new TqPathwayAssessmentBuilder().Build(registrationProfile.TqRegistrationPathways.First());
            var tqPathwayAssessment = PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, pathwayAssessment);

            DbContext.SaveChanges();
            return tqPathwayAssessment;
        }

        public TqSpecialismAssessment SeedSpecialismAssessmentData(TqRegistrationProfile registrationProfile)
        {
            var specialism = registrationProfile.TqRegistrationPathways.First().TqRegistrationSpecialisms.First();
            var specialsimAssessment = new TqSpecialismAssessmentBuilder().Build(specialism);
            var tqSpecialismAssessment = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialsimAssessment);
            DbContext.SaveChanges();
            return tqSpecialismAssessment;
        }

        public List<TqPathwayAssessment> GetPathwayAssessmentsDataToProcess(List<TqRegistrationPathway> pathwayRegistrations)
        {
            var tqPathwayAssessments = new List<TqPathwayAssessment>();

            foreach (var (pathwayRegistration, index) in pathwayRegistrations.Select((value, i) => (value, i)))
            {
                var pathwayAssessment = new TqPathwayAssessmentBuilder().Build(pathwayRegistration, AssessmentSeries[index]);
                var tqPathwayAssessment = PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, pathwayAssessment);
                tqPathwayAssessments.Add(tqPathwayAssessment);
            }
            return tqPathwayAssessments;
        }

        public List<TqSpecialismAssessment> GetSpecialismAssessmentsDataToProcess(List<TqRegistrationSpecialism> specialismRegistrations)
        {
            var tqSpecialismAssessments = new List<TqSpecialismAssessment>();

            foreach (var (specialismRegistration, index) in specialismRegistrations.Select((value, i) => (value, i)))
            {
                var specialismAssessment = new TqSpecialismAssessmentBuilder().Build(specialismRegistration, AssessmentSeries[index]);
                var tqSpecialismAssessment = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialismAssessment);
                tqSpecialismAssessments.Add(tqSpecialismAssessment);
            }
            return tqSpecialismAssessments;
        }

        public void Dispose()
        {
            DbCheckpoint.Reset(TestDatabaseConfiguration.GetConnectionString()).GetAwaiter().GetResult();
            DbContext?.Dispose();
        }

        public async Task ResetData()
        {
            await DbCheckpoint.Reset(TestDatabaseConfiguration.GetConnectionString());
        }
    }
}
