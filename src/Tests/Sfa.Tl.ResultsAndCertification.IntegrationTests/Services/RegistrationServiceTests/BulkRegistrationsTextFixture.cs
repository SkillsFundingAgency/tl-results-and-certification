﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Respawn;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class BulkRegistrationsTextFixture : IDisposable
    {
        public long Uln;
        public TlAwardingOrganisation TlAwardingOrganisation;
        protected IList<TlSpecialism> Specialisms;
        protected IList<TlProvider> TlProviders;
        public IList<TqProvider> TqProviders;
        protected IList<TlSpecialism> TlSpecialisms;
        private IList<AssessmentSeries> AssessmentSeries;
        protected IList<AcademicYear> AcademicYears;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<TlLookup> SpecialismComponentGrades;
        protected RegistrationService RegistrationService;
        protected IProviderRepository ProviderRepository;
        protected IRepository<TqRegistrationPathway> TqRegistrationPathwayRepository;
        protected IRepository<TqRegistrationSpecialism> TqRegistrationSpecialismRepository;
        protected IRepository<TqPathwayAssessment> TqPathwayAssessmentRepository;
        protected ILogger<ProviderRepository> ProviderRepositoryLogger;
        protected ILogger<RegistrationRepository> RegistrationRepositoryLogger;
        protected ILogger<GenericRepository<TqRegistrationPathway>> TqRegistrationPathwayRepositoryLogger;
        protected ILogger<GenericRepository<TqRegistrationSpecialism>> TqRegistrationSpecialismRepositoryLogger;
        protected ILogger<GenericRepository<TqPathwayAssessment>> TqPathwayAssessmentRepositoryLogger;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected ICommonService CommonService;
        protected ISystemProvider SystemProvider;
        public IRegistrationRepository RegistrationRepository;
        protected IMapper RegistrationMapper;
        public RegistrationProcessResponse Result;
        public IList<TqRegistrationProfile> TqRegistrationProfilesData;

        public TqRegistrationProfile TqRegistrationProfileBeforeSeed;

        public bool RejoinResult;
        public Checkpoint DbCheckpoint;
        public ResultsAndCertificationDbContext DbContext;

        protected ILogger<CommonService> CommonServiceLogger;
        protected IRepository<TlLookup> TlLookupRepository;
        protected IRepository<FunctionLog> FunctionLogRepository;
        protected ICommonRepository CommonRepository;
        protected ResultsAndCertificationConfiguration Configuration;
        protected ILogger<GenericRepository<TlLookup>> TlLookupRepositoryLogger;
        protected ILogger<GenericRepository<FunctionLog>> FunctionLogRepositoryLogger;
        protected IAsyncNotificationClient NotificationsClient;
        protected ILogger<NotificationService> NotificationLogger;
        protected IRepository<NotificationTemplate> NotificationTemplateRepository;
        protected ILogger<GenericRepository<NotificationTemplate>> NotificationTemplateRepositoryLogger;
        protected ILogger<INotificationService> NotificationServiceLogger;
        protected INotificationService NotificationService;
        protected IMapper CommonMapper;
        protected ILogger<GenericRepository<ChangeLog>> ChangeLogRepositoryLogger;
        protected IRepository<ChangeLog> ChangeLogRepository;

        public BulkRegistrationsTextFixture()
        {
            DbCheckpoint = new Checkpoint { WithReseed = true };
            DbContext = TestDatabaseConfiguration.CreateRelationalDbContext();
            Initialize();
        }

        public void Initialize()
        {
            CreateMapper();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            TqPathwayAssessmentRepositoryLogger = new Logger<GenericRepository<TqPathwayAssessment>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            TqPathwayAssessmentRepository = new GenericRepository<TqPathwayAssessment>(TqPathwayAssessmentRepositoryLogger, DbContext);

            CommonServiceLogger = new Logger<CommonService>(new NullLoggerFactory());
            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);
            FunctionLogRepositoryLogger = new Logger<GenericRepository<FunctionLog>>(new NullLoggerFactory());
            FunctionLogRepository = new GenericRepository<FunctionLog>(FunctionLogRepositoryLogger, DbContext);
            CommonRepository = new CommonRepository(DbContext);

            NotificationsClient = Substitute.For<IAsyncNotificationClient>();
            NotificationLogger = new Logger<NotificationService>(new NullLoggerFactory());
            NotificationTemplateRepositoryLogger = new Logger<GenericRepository<NotificationTemplate>>(new NullLoggerFactory());
            NotificationTemplateRepository = new GenericRepository<NotificationTemplate>(NotificationTemplateRepositoryLogger, DbContext);
            NotificationService = new NotificationService(NotificationTemplateRepository, NotificationsClient, NotificationLogger);
            ChangeLogRepositoryLogger = new Logger<GenericRepository<ChangeLog>>(new NullLoggerFactory());
            ChangeLogRepository = new GenericRepository<ChangeLog>(ChangeLogRepositoryLogger, DbContext);

            CommonService = new CommonService(CommonServiceLogger, CommonMapper, TlLookupRepository, FunctionLogRepository, CommonRepository, NotificationService, Configuration, ChangeLogRepository);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqPathwayAssessmentRepository, TqRegistrationSpecialismRepository, CommonService, SystemProvider, RegistrationMapper, RegistrationRepositoryLogger);

            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null);
            DbContext.SaveChanges();
        }

        protected void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(RegistrationMapper).Assembly));
            RegistrationMapper = new Mapper(mapperConfig);

            var commonMapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(CommonMapper).Assembly));
            CommonMapper = new Mapper(commonMapperConfig);
        }

        public async Task WhenReJoinAsync(RejoinRegistrationRequest rejoinRegistrationRequest)
        {
            RejoinResult = await RegistrationService.RejoinRegistrationAsync(rejoinRegistrationRequest);
        }

        public async Task WhenAsync()
        {
            Result = await RegistrationService.CompareAndProcessRegistrationsAsync(TqRegistrationProfilesData);
        }

        public void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson)
        {
            AcademicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext).ToList();
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            var routes = TlevelDataProvider.CreateTlRoutes(DbContext, awardingOrganisation);
            var pathways = TlevelDataProvider.CreateTlPathways(DbContext, awardingOrganisation, routes);
            TlSpecialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, pathways.First());

            var tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, awardingOrganisation, TlAwardingOrganisation, pathways);
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

            TlLookup = TlLookupDataProvider.CreateTlLookupList(DbContext, null, true);
            PathwayComponentGrades = TlLookup.Where(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            SpecialismComponentGrades = TlLookupDataProvider.CreateSpecialismGradeTlLookupList(DbContext, null, true);
            DbContext.SaveChanges();
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, TqProvider tqProvider = null, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, bool isBulkUpload = true, bool seedIndustryPlacement = false)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());

            if (seedIndustryPlacement)
            {
                var industryPlacement = IndustryPlacementProvider.CreateIndustryPlacement(DbContext, new IndustryPlacement { Status = IndustryPlacementStatus.Completed, Details = "{'industryPlacementStatus': 'Completed'}", CreatedBy = "Test User" });
                tqRegistrationPathway.IndustryPlacements.Add(industryPlacement);
            }

            tqRegistrationPathway.IsBulkUpload = isBulkUpload;


            foreach (var specialism in TlSpecialisms)
            {
                var tqSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, specialism);
                tqSpecialism.IsBulkUpload = isBulkUpload;
                tqRegistrationPathway.TqRegistrationSpecialisms.Add(tqSpecialism);
            }

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.Status = status;
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);

                foreach (var tqRegistrationSpecialism in tqRegistrationPathway.TqRegistrationSpecialisms)
                {
                    tqRegistrationSpecialism.IsOptedin = true;
                    tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
                }
            }

            DbContext.SaveChanges();
            return profile;
        }

        public TqRegistrationProfile GetRegistrationsDataToProcess(long uln, TqProvider tqProvider = null)
        {
            return GetRegistrationsDataToProcess(new List<long> { uln }, tqProvider).First();
        }

        public List<TqRegistrationProfile> GetRegistrationsDataToProcess(List<long> ulns, TqProvider tqProvider = null)
        {
            var tqRegistrationProfiles = new List<TqRegistrationProfile>();

            foreach (var (uln, index) in ulns.Select((value, i) => (value, i)))
            {
                tqProvider ??= TqProviders[index];
                var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile, false);
                var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider, false);

                foreach (var specialism in TlSpecialisms)
                {
                    tqRegistrationPathway.TqRegistrationSpecialisms.Add(RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, specialism, addToDbContext: false));
                }

                tqRegistrationProfile.TqRegistrationPathways = new List<TqRegistrationPathway> { tqRegistrationPathway };
                tqRegistrationProfiles.Add(tqRegistrationProfile);
            }
            return tqRegistrationProfiles;
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

        public List<TqPathwayAssessment> GetPathwayAssessmentsDataToProcess(List<TqRegistrationPathway> pathwayRegistrations, bool seedPathwayAssessmentsAsActive = true, bool isHistorical = false, bool isBulkUpload = true)
        {
            var tqPathwayAssessments = new List<TqPathwayAssessment>();

            foreach (var (pathwayRegistration, index) in pathwayRegistrations.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var pathwayAssessment = new TqPathwayAssessmentBuilder().Build(pathwayRegistration, AssessmentSeries[index], isBulkUpload);
                    pathwayAssessment.IsOptedin = false;
                    pathwayAssessment.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqPathwayAssessmentHistorical = PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, pathwayAssessment);
                    tqPathwayAssessments.Add(tqPathwayAssessmentHistorical);
                }

                var activePathwayAssessment = new TqPathwayAssessmentBuilder().Build(pathwayRegistration, AssessmentSeries[index], isBulkUpload);
                var tqPathwayAssessment = PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, activePathwayAssessment);
                if (!seedPathwayAssessmentsAsActive)
                {
                    tqPathwayAssessment.IsOptedin = pathwayRegistration.Status == RegistrationPathwayStatus.Withdrawn ? true : false;
                    tqPathwayAssessment.EndDate = DateTime.UtcNow;
                }

                tqPathwayAssessments.Add(tqPathwayAssessment);
            }
            return tqPathwayAssessments;
        }

        public List<TqPathwayAssessment> SeedPathwayAssessmentsData(List<TqPathwayAssessment> pathwayAssessments, bool saveChanges = true)
        {
            var tqPathwayAssessment = PathwayAssessmentDataProvider.CreateTqPathwayAssessments(DbContext, pathwayAssessments);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqPathwayAssessment;
        }

        public List<TqSpecialismResult> GetSpecialismResultsDataToProcess(List<TqSpecialismAssessment> specialismAssessments, bool seedSpecialismResultsAsActive = true, bool isHistorical = false, bool isBulkUpload = true)
        {
            var tqSpecialismResults = new List<TqSpecialismResult>();

            foreach (var (specialismAssessment, index) in specialismAssessments.Select((value, i) => (value, i)))
            {
                var tqresults = GetSpecialismResultDataToProcess(specialismAssessment, seedSpecialismResultsAsActive, isHistorical, isBulkUpload);
                tqSpecialismResults.AddRange(tqresults);
            }

            return tqSpecialismResults;
        }

        public List<TqPathwayResult> GetPathwayResultsDataToProcess(List<TqPathwayAssessment> pathwayAssessments, bool seedPathwayResultsAsActive = true, bool isHistorical = false, bool isBulkUpload = true)
        {
            var tqPathwayResults = new List<TqPathwayResult>();

            foreach (var (pathwayAssessment, index) in pathwayAssessments.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var pathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment, PathwayComponentGrades[index], isBulkUpload);
                    pathwayResult.IsOptedin = false;
                    pathwayResult.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqPathwayResultHistorical = TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, pathwayResult);
                    tqPathwayResults.Add(tqPathwayResultHistorical);
                }

                var activePathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment, PathwayComponentGrades[index], isBulkUpload);
                var tqPathwayResult = TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, activePathwayResult);
                if (!seedPathwayResultsAsActive)
                {
                    tqPathwayResult.IsOptedin = pathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? true : false;
                    tqPathwayResult.EndDate = DateTime.UtcNow;
                }

                tqPathwayResults.Add(tqPathwayResult);
            }
            return tqPathwayResults;
        }

        public List<TqPathwayResult> SeedPathwayResultsData(List<TqPathwayResult> pathwayResults, bool saveChanges = true)
        {
            var tqPathwayResult = TqPathwayResultDataProvider.CreateTqPathwayResults(DbContext, pathwayResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqPathwayResult;
        }

        public IEnumerable<TqSpecialismAssessment> GetSpecialismAssessmentsDataToProcess(IEnumerable<TqRegistrationSpecialism> registrationSpecialisms, bool seedSpecialismAssessmentsAsActive = true, bool isHistorical = false, bool isBulkUpload = true)
        {
            var tqSpecialismAssessments = new List<TqSpecialismAssessment>();

            foreach (var (regSpecialism, index) in registrationSpecialisms.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var specialismAssessment = new TqSpecialismAssessmentBuilder().Build(regSpecialism, AssessmentSeries[index], isBulkUpload);
                    specialismAssessment.IsOptedin = false;
                    specialismAssessment.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqPathwayAssessmentHistorical = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialismAssessment);
                    tqSpecialismAssessments.Add(tqPathwayAssessmentHistorical);
                }

                var activeSpecialismAssessment = new TqSpecialismAssessmentBuilder().Build(regSpecialism, AssessmentSeries[index], isBulkUpload);
                var tqSpecialismAssessment = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, activeSpecialismAssessment);
                if (!seedSpecialismAssessmentsAsActive)
                {
                    tqSpecialismAssessment.IsOptedin = regSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? true : false;
                    tqSpecialismAssessment.EndDate = DateTime.UtcNow;
                }

                tqSpecialismAssessments.Add(tqSpecialismAssessment);
            }
            return tqSpecialismAssessments;
        }

        public List<TqSpecialismAssessment> SeedSpecialismAssessmentsData(List<TqSpecialismAssessment> specialismAssessments, bool saveChanges = true)
        {
            var tqSpecialismAssessment = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessments(DbContext, specialismAssessments);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismAssessment;
        }

        public List<TqSpecialismResult> SeedSpecialismResultsData(List<TqSpecialismResult> specialismResults, bool saveChanges = true)
        {
            var tqSpecialismResults = TqSpecialismResultDataProvider.CreateTqSpecialismResults(DbContext, specialismResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismResults;
        }

        public PrintBatchItem SeedPrintBatchItem()
        {
            var printBatchItemBuilder = new PrintBatchItemBuilder();
            var printBatchItem = printBatchItemBuilder.Build();

            DbContext.Add(printBatchItem);
            DbContext.SaveChanges();

            return printBatchItem;
        }

        public List<TqSpecialismResult> GetSpecialismResultDataToProcess(TqSpecialismAssessment specialismAssessment, bool seedSpecialismResultsAsActive = true, bool isHistorical = false, bool isBulkUpload = true)
        {
            var tqSpecialismResults = new List<TqSpecialismResult>();

            if (isHistorical)
            {
                // Historical record
                var specialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment, SpecialismComponentGrades[0], isBulkUpload: isBulkUpload);
                specialismResult.IsOptedin = false;
                specialismResult.EndDate = DateTime.UtcNow.AddDays(-1);

                var tqSpecialismResultHistorical = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, specialismResult);
                tqSpecialismResults.Add(tqSpecialismResultHistorical);
            }

            var activeSpecialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment, SpecialismComponentGrades[0], isBulkUpload: isBulkUpload);
            var tqSpecialismResult = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, activeSpecialismResult);
            if (!seedSpecialismResultsAsActive)
            {
                tqSpecialismResult.IsOptedin = specialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? true : false;
                tqSpecialismResult.EndDate = DateTime.UtcNow;
            }

            tqSpecialismResults.Add(tqSpecialismResult);
            return tqSpecialismResults;
        }
    }
}
