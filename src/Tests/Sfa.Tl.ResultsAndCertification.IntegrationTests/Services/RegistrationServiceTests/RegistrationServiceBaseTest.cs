using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public abstract class RegistrationServiceBaseTest : BaseTest<TqRegistrationPathway>
    {
        protected RegistrationService RegistrationService;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected IList<TlSpecialism> Specialisms;
        protected TlProvider TlProvider;
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected List<TqPathwayAssessment> TqPathwayAssessment;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<AcademicYear> AcademicYears;
        protected TqProvider TqProvider;
        protected IList<TlProvider> TlProviders;
        protected IList<TqProvider> TqProviders;
        protected IList<TlLookup> TlLookup;
        protected IList<Qualification> Qualifications;
        protected List<TlPathwaySpecialismCombination> TlPathwaySpecialismCombinations;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected IProviderRepository ProviderRepository;
        protected IRegistrationRepository RegistrationRepository;
        protected IRepository<TqRegistrationPathway> TqRegistrationPathwayRepository;
        protected IRepository<TqRegistrationSpecialism> TqRegistrationSpecialismRepository;
        protected ILogger<ProviderRepository> ProviderRepositoryLogger;
        protected ILogger<RegistrationRepository> RegistrationRepositoryLogger;
        protected ILogger<GenericRepository<TqRegistrationPathway>> TqRegistrationPathwayRepositoryLogger;
        protected ILogger<GenericRepository<TqRegistrationSpecialism>> TqRegistrationSpecialismRepositoryLogger;
        protected ICommonService CommonService;
        protected IMapper RegistrationMapper;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(RegistrationMapper).Assembly));
            RegistrationMapper = new Mapper(mapperConfig);
        }

        protected void CreateCommonService()
        {
            var commonServiceLogger = new Logger<CommonService>(new NullLoggerFactory());
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(CommonMapper).Assembly));
            var commonMapper = new Mapper(mapperConfig);

            var tlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            var tlLookupRepository = new GenericRepository<TlLookup>(tlLookupRepositoryLogger, DbContext);

            var functionLogRepositoryLogger = new Logger<GenericRepository<FunctionLog>>(new NullLoggerFactory());
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogRepositoryLogger, DbContext);
            var commonRepository = new CommonRepository(DbContext);

            var notificationsClient = Substitute.For<IAsyncNotificationClient>();
            var notificationLogger = new Logger<NotificationService>(new NullLoggerFactory());
            var notificationTemplateRepositoryLogger = new Logger<GenericRepository<NotificationTemplate>>(new NullLoggerFactory());
            var notificationTemplateRepository = new GenericRepository<NotificationTemplate>(notificationTemplateRepositoryLogger, DbContext);
            var notificationService = new NotificationService(notificationTemplateRepository, notificationsClient, notificationLogger);

            var configuration = new ResultsAndCertificationConfiguration { TlevelQueriedSupportEmailAddress = "test@test.com" };

            CommonService = new CommonService(commonServiceLogger, commonMapper, tlLookupRepository, functionLogRepository, commonRepository, notificationService, configuration);
        } 

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway);
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProvider);
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            AcademicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);

            var combinations = new TlPathwaySpecialismCombinationBuilder().BuildList();
            TlPathwaySpecialismCombinations = new List<TlPathwaySpecialismCombination>();
            foreach (var (specialism, index) in Specialisms.Take(combinations.Count).Select((value, i) => (value, i)))
            {
                combinations[index].TlPathwayId = Pathway.Id;
                combinations[index].TlPathway = Pathway;
                combinations[index].TlSpecialismId = specialism.Id;
                combinations[index].TlSpecialism = specialism;
                TlPathwaySpecialismCombinations.AddRange(TlevelDataProvider.CreateTlPathwaySpecialismCombinationsList(DbContext, combinations));
            }
            DbContext.SaveChangesAsync();
        }

        public List<TqRegistrationProfile> SeedRegistrationsDataByStatus(Dictionary<long, RegistrationPathwayStatus> ulns, TqProvider tqProvider = null)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedRegistrationDataByStatus(uln.Key, uln.Value, tqProvider));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationDataByStatus(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null, bool saveChanges = true)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProvider);
            tqRegistrationPathway.Status = status;

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
            }

            foreach (var specialism in Specialisms)
            {
                tqRegistrationPathway.TqRegistrationSpecialisms.Add(RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, specialism));
                if (status == RegistrationPathwayStatus.Withdrawn)
                    tqRegistrationPathway.TqRegistrationSpecialisms.ToList().ForEach(s => { s.EndDate = DateTime.UtcNow.AddDays(-1); });
            }

            if (saveChanges)
                DbContext.SaveChanges();
            
            return tqRegistrationProfile;
        }

        public List<TqPathwayAssessment> SeedPathwayAssessmentsData(List<TqPathwayAssessment> pathwayAssessments, bool saveChanges = true)
        {
            TqPathwayAssessment = PathwayAssessmentDataProvider.CreateTqPathwayAssessments(DbContext, pathwayAssessments);
            if (saveChanges)
                DbContext.SaveChanges();

            return TqPathwayAssessment;
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

        public List<TqPathwayResult> GetPathwayResultsDataToProcess(List<TqPathwayAssessment> pathwayAssessments, bool seedPathwayResultsAsActive = true, bool isHistorical = false, bool isBulkUpload = true)
        {
            var tqPathwayResults = new List<TqPathwayResult>();

            foreach (var (pathwayAssessment, index) in pathwayAssessments.Select((value, i) => (value, i)))
            {
                var tqresults = GetPathwayResultDataToProcess(pathwayAssessment, seedPathwayResultsAsActive, isHistorical, isBulkUpload);
                tqPathwayResults.AddRange(tqresults);
            }
            
            return tqPathwayResults;
        }

        public List<TqPathwayResult> GetPathwayResultDataToProcess(TqPathwayAssessment pathwayAssessment, bool seedPathwayResultsAsActive = true, bool isHistorical = false, bool isBulkUpload = true)
        {
            var tqPathwayResults = new List<TqPathwayResult>();

            if (isHistorical)
            {
                // Historical record
                var pathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment, isBulkUpload: isBulkUpload);
                pathwayResult.IsOptedin = false;
                pathwayResult.EndDate = DateTime.UtcNow.AddDays(-1);

                var tqPathwayResultHistorical = TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, pathwayResult);
                tqPathwayResults.Add(tqPathwayResultHistorical);
            }

            var activePathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment, isBulkUpload: isBulkUpload);
            var tqPathwayResult = TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, activePathwayResult);
            if (!seedPathwayResultsAsActive)
            {
                tqPathwayResult.IsOptedin = pathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? true : false;
                tqPathwayResult.EndDate = DateTime.UtcNow;
            }

            tqPathwayResults.Add(tqPathwayResult);
            return tqPathwayResults;
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

        public List<TqSpecialismResult> GetSpecialismResultDataToProcess(TqSpecialismAssessment specialismAssessment, bool seedSpecialismResultsAsActive = true, bool isHistorical = false, bool isBulkUpload = true)
        {
            var tqSpecialismResults = new List<TqSpecialismResult>();

            if (isHistorical)
            {
                // Historical record
                var specialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment, isBulkUpload: isBulkUpload);
                specialismResult.IsOptedin = false;
                specialismResult.EndDate = DateTime.UtcNow.AddDays(-1);

                var tqSpecialismResultHistorical = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, specialismResult);
                tqSpecialismResults.Add(tqSpecialismResultHistorical);
            }

            var activeSpecialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment, isBulkUpload: isBulkUpload);
            var tqSpecialismResult = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, activeSpecialismResult);
            if (!seedSpecialismResultsAsActive)
            {
                tqSpecialismResult.IsOptedin = specialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? true : false;
                tqSpecialismResult.EndDate = DateTime.UtcNow;
            }

            tqSpecialismResults.Add(tqSpecialismResult);
            return tqSpecialismResults;
        }


        public List<TqPathwayResult> SeedPathwayResultsData(List<TqPathwayResult> pathwayResults, bool saveChanges = true)
        {
            var tqPathwayResults = TqPathwayResultDataProvider.CreateTqPathwayResults(DbContext, pathwayResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqPathwayResults;
        }

        public List<TqSpecialismResult> SeedSpecialismResultsData(List<TqSpecialismResult> specialismResults, bool saveChanges = true)
        {
            var tqSpecialismResults = TqSpecialismResultDataProvider.CreateTqSpecialismResults(DbContext, specialismResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismResults;
        }

        public List<TqSpecialismAssessment> GetSpecialismAssessmentsDataToProcess(List<TqRegistrationSpecialism> specialismRegistrations, bool seedSpecialismAssessmentsAsActive = true, bool isHistorical = false, bool isBulkUpload = true)
        {
            var tqSpecialismAssessments = new List<TqSpecialismAssessment>();

            foreach (var (specialismRegistration, index) in specialismRegistrations.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var specialismAssessment = new TqSpecialismAssessmentBuilder().Build(specialismRegistration, AssessmentSeries[index], isBulkUpload);
                    specialismAssessment.IsOptedin = false;
                    specialismAssessment.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqSpecialismAssessmentHistorical = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialismAssessment);
                    tqSpecialismAssessments.Add(tqSpecialismAssessmentHistorical);
                }

                var activeSpecialismAssessment = new TqSpecialismAssessmentBuilder().Build(specialismRegistration, AssessmentSeries[index], isBulkUpload);
                var tqSpecialismAssessment = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, activeSpecialismAssessment);
                if (!seedSpecialismAssessmentsAsActive)
                {
                    tqSpecialismAssessment.IsOptedin = specialismRegistration.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn;
                    tqSpecialismAssessment.EndDate = DateTime.UtcNow;
                }
                tqSpecialismAssessments.Add(tqSpecialismAssessment);

            }
            return tqSpecialismAssessments;
        }

        public List<TqSpecialismAssessment> SeedSpecialismAssessmentsData(List<TqSpecialismAssessment> specialismAssessments, bool saveChanges = true)
        {
            var tqSpecialismAssessments = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessments(DbContext, specialismAssessments);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismAssessments;
        }

        public void SeedQualificationAchievedData(TqRegistrationProfile profile)
        {
            var engQual = Qualifications.FirstOrDefault(e => e.TlLookup.Code == "Eng");
            var mathQual = Qualifications.FirstOrDefault(e => e.TlLookup.Code == "Math");

            var engQualifcationGrade = engQual.QualificationType.QualificationGrades.FirstOrDefault(x => x.IsAllowable);
            var mathsQualifcationGrade = mathQual.QualificationType.QualificationGrades.FirstOrDefault(x => x.IsAllowable);

            profile.QualificationAchieved.Add(new QualificationAchieved
            {
                TqRegistrationProfileId = profile.Id,
                QualificationId = engQual.Id,
                QualificationGradeId = engQualifcationGrade.Id,
                IsAchieved = engQualifcationGrade.IsAllowable
            });

            profile.QualificationAchieved.Add(new QualificationAchieved
            {
                TqRegistrationProfileId = profile.Id,
                QualificationId = mathQual.Id,
                QualificationGradeId = mathsQualifcationGrade.Id,
                IsAchieved = mathsQualifcationGrade.IsAllowable
            });
            DbContext.SaveChanges();
        }

        public IList<Qualification> SeedQualificationData()
        {
            var qualificationsList = new QualificationBuilder().BuildList();
            var qualifications = QualificationDataProvider.CreateQualificationList(DbContext, qualificationsList);

            foreach (var qual in qualifications)
            {
                qual.QualificationType.QualificationGrades = new QualificationGradeBuilder().BuildList(qual.QualificationType);
            }
            return qualifications;
        }

        public static void AssertRegistrationPathway(TqRegistrationPathway actualPathway, TqRegistrationPathway expectedPathway, bool assertStatus = true, bool isTransferred = false)
        {
            actualPathway.Should().NotBeNull();

            if(!isTransferred)
                actualPathway.TqProviderId.Should().Be(expectedPathway.TqProviderId);

            actualPathway.AcademicYear.Should().Be(expectedPathway.AcademicYear);

            if (assertStatus)
                actualPathway.Status.Should().Be(expectedPathway.Status);

            actualPathway.IsBulkUpload.Should().Be(expectedPathway.IsBulkUpload);

            // Assert specialisms
            actualPathway.TqRegistrationSpecialisms.Count.Should().Be(expectedPathway.TqRegistrationSpecialisms.Count);

            foreach (var expectedSpecialism in expectedPathway.TqRegistrationSpecialisms)
            {
                var actualSpecialism = actualPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.TlSpecialismId == expectedSpecialism.TlSpecialismId);

                actualSpecialism.Should().NotBeNull();
                actualSpecialism.TlSpecialismId.Should().Be(expectedSpecialism.TlSpecialismId);
                actualSpecialism.IsOptedin.Should().Be(expectedSpecialism.IsOptedin);
                actualSpecialism.IsBulkUpload.Should().Be(expectedSpecialism.IsBulkUpload);
            }
        }

        public static void AssertPathwayAssessment(TqPathwayAssessment actualAssessment, TqPathwayAssessment expectedAssessment, bool isRejoin = false, bool isTransferred = false)
        {
            actualAssessment.Should().NotBeNull();
            if (!isRejoin && !isTransferred)
                actualAssessment.TqRegistrationPathwayId.Should().Be(expectedAssessment.TqRegistrationPathwayId);

            if(!isTransferred)
                actualAssessment.TqRegistrationPathway.TqProviderId.Should().Be(expectedAssessment.TqRegistrationPathway.TqProviderId);

            actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId);
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeFalse();

            if (actualAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualAssessment.EndDate.Should().BeNull();
            else
                actualAssessment.EndDate.Should().NotBeNull();
        }

        public static void AssertPathwayResults(TqPathwayResult actualResult, TqPathwayResult expectedResult, bool isRejoin = false, bool isTransferred = false)
        {
            actualResult.Should().NotBeNull();
            if (!isRejoin && !isTransferred)
            actualResult.TqPathwayAssessmentId.Should().Be(expectedResult.TqPathwayAssessmentId);

            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualResult.IsOptedin.Should().BeTrue();
            actualResult.IsBulkUpload.Should().BeFalse();

            if (actualResult.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualResult.EndDate.Should().BeNull();
            else
                actualResult.EndDate.Should().NotBeNull();
        }

        public void AssertSpecialismAssessment(IEnumerable<TqSpecialismAssessment> actualSpecialismAssessment, IEnumerable<TqSpecialismAssessment> expectedSpecialismAssessment)
        {
            actualSpecialismAssessment.Should().NotBeEmpty();
            actualSpecialismAssessment.Should().HaveSameCount(expectedSpecialismAssessment);

            actualSpecialismAssessment.ToList().ForEach(actualAssessment =>
            {
                var expectedAssessment = expectedSpecialismAssessment.FirstOrDefault(x => x.Id == actualAssessment.Id);
                expectedAssessment.Should().NotBeNull();

                actualAssessment.TqRegistrationSpecialismId.Should().Be(expectedAssessment.TqRegistrationSpecialismId);
                actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId);
                actualAssessment.IsOptedin.Should().Be(expectedAssessment.IsOptedin);
                actualAssessment.IsBulkUpload.Should().Be(expectedAssessment.IsBulkUpload);
                actualAssessment.StartDate.Should().Be(expectedAssessment.StartDate);
                actualAssessment.CreatedBy.Should().Be(expectedAssessment.CreatedBy);
                if (expectedAssessment.EndDate != null)
                    actualAssessment.EndDate.Value.ToShortDateString().Should().Be(expectedAssessment.EndDate.Value.ToShortDateString());
                else
                    actualAssessment.EndDate.Should().BeNull();
            });
        }

        public static void AssertSpecialismAssessment(TqSpecialismAssessment actualAssessment, TqSpecialismAssessment expectedAssessment, bool isRejoin = false, bool isTransferred = false)
        {
            actualAssessment.Should().NotBeNull();
            if (!isRejoin && !isTransferred)
                actualAssessment.TqRegistrationSpecialismId.Should().Be(expectedAssessment.TqRegistrationSpecialismId);

            actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId);
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeFalse();

            if (actualAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualAssessment.EndDate.Should().BeNull();
            else
                actualAssessment.EndDate.Should().NotBeNull();
        }

        public static void AssertSpecialismResult(TqSpecialismResult actualResult, TqSpecialismResult expectedResult, bool isRejoin = false, bool isTransferred = false)
        {
            actualResult.Should().NotBeNull();
            if (!isRejoin && !isTransferred)
                actualResult.TqSpecialismAssessmentId.Should().Be(expectedResult.TqSpecialismAssessmentId);

            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualResult.IsOptedin.Should().BeTrue();
            actualResult.IsBulkUpload.Should().BeFalse();

            if (actualResult.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualResult.EndDate.Should().BeNull();
            else
                actualResult.EndDate.Should().NotBeNull();
        }
    }
}
