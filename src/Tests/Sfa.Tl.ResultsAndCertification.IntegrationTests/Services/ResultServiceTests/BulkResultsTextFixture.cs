﻿using AutoMapper;
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
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
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
        protected IList<AssessmentSeries> AssessmentSeries;
        public Checkpoint DbCheckpoint;
        public ResultsAndCertificationDbContext DbContext;
        protected IAssessmentRepository AssessmentRepository;
        protected ILogger<AssessmentRepository> AssessmentRepositoryLogger;
        protected IRepository<TlLookup> TlLookupRepository;
        protected ILogger<GenericRepository<TlLookup>> TlLookupRepositoryLogger;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;

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
            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);

            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);

            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);

            ResultRepositoryLogger = new Logger<ResultRepository>(new NullLoggerFactory());
            ResultRepository = new ResultRepository(ResultRepositoryLogger, DbContext);

            // Service
            ResultService = new ResultService(AssessmentRepository, AssessmentSeriesRepository, TlLookupRepository, ResultRepository, ResultMapper);
        }

        protected void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(RegistrationMapper).Assembly));
            ResultMapper = new Mapper(mapperConfig);
        }

        public async Task WhenAsync()
        {
            Result = await ResultService.CompareAndProcessResultsAsync(TqPathwayResultsData);
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

            var tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, awardingOrganisation, tlAwardingOrganisation, pathways);
            DbContext.SaveChanges();

            PathwayComponentGrades = TlLookup.Where(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
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
