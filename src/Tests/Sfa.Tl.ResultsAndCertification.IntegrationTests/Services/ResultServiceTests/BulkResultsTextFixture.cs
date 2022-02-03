using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Respawn;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ResultServiceTests
{
    public class BulkResultsTextFixture : IDisposable
    {
        //public long Uln;
        public List<long> Ulns;
        protected IList<TlProvider> TlProviders;
        public IList<TqProvider> TqProviders;
        protected IList<TlSpecialism> TlSpecialisms;
        public IList<TqPathwayAssessment> TqPathwayAssessmentsData;
        public IList<TqSpecialismAssessment> TqSpecialismAssessmentsData;
        protected ResultService ResultService;
        protected ILogger<ResultRepository> ResultRepositoryLogger;
        protected ILogger<GenericRepository<AssessmentSeries>> AssessmentSeriesRepositoryLogger;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        public IResultRepository ResultRepository;

        protected IRepository<TqPathwayAssessment> PathwayAssessmentRepository;
        protected IRepository<TqSpecialismAssessment> SpecialismAssessmentRepository;
        protected IRepository<AssessmentSeries> AssessmentSeriesRepository;
        protected IMapper ResultMapper;
        public ResultProcessResponse Result;
        public IList<TqRegistrationProfile> TqRegistrationProfilesData;
        public IList<TqPathwayResult> TqPathwayResultsData;
        public IList<TqSpecialismResult> TqSpecialismResultsData;
        protected IList<AssessmentSeries> AssessmentSeries;
        public Checkpoint DbCheckpoint;
        public ResultsAndCertificationDbContext DbContext;
        protected IRepository<TqPathwayResult> PathwayResultRepository;
        protected ILogger<GenericRepository<TqPathwayResult>> PathwayResultRepositoryLogger;
        protected IRepository<TqSpecialismResult> SpecialismResultRepository;
        protected ILogger<GenericRepository<TqSpecialismResult>> SpecialismResultRepositoryLogger;
        protected IRepository<TlLookup> TlLookupRepository;
        protected ILogger<GenericRepository<TlLookup>> TlLookupRepositoryLogger;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<TlLookup> SpecialismComponentGrades;
        protected ILogger<ResultService> Logger;

        public BulkResultsTextFixture()
        {
            DbCheckpoint = new Checkpoint { WithReseed = true };
            DbContext = TestDatabaseConfiguration.CreateRelationalDbContext();
            Initialize();
        }

        public void Initialize()
        {
            CreateMapper();
            // Dependencies 
            PathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultRepository = new GenericRepository<TqPathwayResult>(PathwayResultRepositoryLogger, DbContext);

            SpecialismResultRepositoryLogger = new Logger<GenericRepository<TqSpecialismResult>>(new NullLoggerFactory());
            SpecialismResultRepository = new GenericRepository<TqSpecialismResult>(SpecialismResultRepositoryLogger, DbContext);

            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);

            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);

            ResultRepositoryLogger = new Logger<ResultRepository>(new NullLoggerFactory());
            ResultRepository = new ResultRepository(ResultRepositoryLogger, DbContext);

            Logger = new Logger<ResultService>(new NullLoggerFactory());
            // Service
            ResultService = new ResultService(AssessmentSeriesRepository, TlLookupRepository, ResultRepository, PathwayResultRepository, SpecialismResultRepository, ResultMapper, Logger);
        }

        protected void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(RegistrationMapper).Assembly));
            ResultMapper = new Mapper(mapperConfig);
        }

        public async Task WhenAsync()
        {
            Result = await ResultService.CompareAndProcessResultsAsync(TqPathwayResultsData, TqSpecialismResultsData);
        }

        public void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson)
        {
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext).ToList();
            var tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            var routes = TlevelDataProvider.CreateTlRoutes(DbContext, awardingOrganisation);
            var pathways = TlevelDataProvider.CreateTlPathways(DbContext, awardingOrganisation, routes);
            TlSpecialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, pathways.First());
            TlLookup = TlLookupDataProvider.CreateTlLookupList(DbContext);
            var specialismGradesLookup = TlLookupDataProvider.CreateSpecialismGradeTlLookupList(DbContext);
            var tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, awardingOrganisation, tlAwardingOrganisation, pathways);
            DbContext.SaveChanges();

            PathwayComponentGrades = TlLookup.Where(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            SpecialismComponentGrades = specialismGradesLookup;

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

            foreach (var uln in ulns)
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

        public List<TqPathwayAssessment> SeedPathwayAssessmentsData(List<TqRegistrationPathway> registrationPathways)
        {
            var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registrationPathways);
            DbContext.SaveChanges();
            return pathwayAssessments;
        }

        public List<TqSpecialismAssessment> SeedSpecialismAssessmentData(List<TqRegistrationSpecialism> registrationSpecialisms)
        {
            var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registrationSpecialisms);
            DbContext.SaveChanges();
            return specialismAssessments;
        }

        public TqSpecialismAssessment SeedSpecialismAssessmentData(TqRegistrationProfile registrationProfile)
        {
            var specialism = registrationProfile.TqRegistrationPathways.First().TqRegistrationSpecialisms.First();
            var specialsimAssessment = new TqSpecialismAssessmentBuilder().Build(specialism);
            var tqSpecialismAssessment = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialsimAssessment);
            DbContext.SaveChanges();
            return tqSpecialismAssessment;
        }

        public List<TqPathwayAssessment> GetPathwayAssessmentsDataToProcess(List<TqRegistrationPathway> registrationPathways)
        {
            var tqPathwayAssessments = new List<TqPathwayAssessment>();

            foreach (var (registrationPathway, index) in registrationPathways.Select((value, i) => (value, i)))
            {
                var pathwayAssessment = new TqPathwayAssessmentBuilder().Build(registrationPathway, AssessmentSeries[index]);
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

        public List<TqPathwayResult> GetPathwayResultsDataToProcess(List<TqPathwayAssessment> pathwayAssessments)
        {
            var tqPathwayResults = new List<TqPathwayResult>();

            foreach (var (pathwayAssessment, index) in pathwayAssessments.Select((value, i) => (value, i)))
            {
                var pathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment, PathwayComponentGrades[index]);
                var tqPathwayResult = TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, pathwayResult);
                tqPathwayResults.Add(tqPathwayResult);
            }
            return tqPathwayResults;
        }

        public List<TqSpecialismResult> GetSpecialismResultsDataToProcess(List<TqSpecialismAssessment> specialismAssessments)
        {
            var tqSpecialismResults = new List<TqSpecialismResult>();

            foreach (var (specialismAssessment, index) in specialismAssessments.Select((value, i) => (value, i)))
            {
                var specialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment, SpecialismComponentGrades[index]);
                var tqSpecialismResult = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, specialismResult);
                tqSpecialismResults.Add(tqSpecialismResult);
            }
            return tqSpecialismResults;
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
