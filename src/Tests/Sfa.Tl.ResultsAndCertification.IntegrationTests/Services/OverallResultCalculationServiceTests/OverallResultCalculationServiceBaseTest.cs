using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Application.Strategies;
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

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public abstract class OverallResultCalculationServiceBaseTest : BaseTest<OverallResult>
    {
        // Seed variables
        protected long AoUkprn = 10011881;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected IList<TlSpecialism> Specialisms;
        protected TlProvider TlProvider;
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TqProvider TqProvider;
        protected IList<TlProvider> TlProviders;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<TlLookup> SpecialismComponentGrades;
        protected IList<AcademicYear> AcademicYears;
        protected IList<OverallGradeLookup> OverallGradeLookup;
        protected IList<DualSpecialismOverallGradeLookup> DualSpecialismOverallGradeLookup;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected OverallResultCalculationService OverallResultCalculationService;

        protected IOverallResultCalculationRepository OverallResultCalculationRepository;

        protected IRepository<TlLookup> TlLookupRepository;
        protected ILogger<GenericRepository<TlLookup>> TlLookupRepositoryLogger;
        protected ILogger<GenericRepository<DualSpecialismOverallGradeLookup>> DualSpecialismOverallGradeLookupLogger;
        protected ILogger<GenericRepository<OverallGradeLookup>> OverallGradeLookupLogger;
        protected IRepository<AssessmentSeries> AssessmentSeriesRepository;
        protected ILogger<GenericRepository<AssessmentSeries>> AssessmentSeriesLogger;
        protected IRepository<OverallResult> OverallResultRepository;
        protected ILogger<GenericRepository<OverallResult>> OverallResultLogger;
        protected IMapper Mapper;

        protected ISpecialismResultStrategyFactory SpecialismResultStrategyFactory;
        protected IOverallGradeStrategyFactory OverallGradeStrategyFactory;

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
            TlLookup = TlLookupDataProvider.CreateFullTlLookupList(DbContext, null, true);
            PathwayComponentGrades = TlLookup.Where(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            SpecialismComponentGrades = TlLookupDataProvider.CreateSpecialismGradeTlLookupList(DbContext, null, true);
            AcademicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            OverallGradeLookup = OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, null, true);
            DualSpecialismOverallGradeLookup = DualSpecialismOverallGradeLookupProvider.DualSpecialismOverallGradeLookupList(DbContext, TlLookup, true);
            DbContext.SaveChanges();
        }

        protected virtual void CreateService()
        {
            // Dependencies
            {
                ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
                {
                    OverallResultBatchSettings = new OverallResultBatchSettings
                    {
                        BatchSize = 2,
                        NoOfAcademicYearsToProcess = 4
                    }
                };
                TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
                TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);
                AssessmentSeriesLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
                AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesLogger, DbContext);
                OverallResultLogger = new Logger<GenericRepository<OverallResult>>(new NullLoggerFactory());
                OverallResultRepository = new GenericRepository<OverallResult>(OverallResultLogger, DbContext);
                OverallResultCalculationRepository = new OverallResultCalculationRepository(DbContext);

                DualSpecialismOverallGradeLookupLogger = new Logger<GenericRepository<DualSpecialismOverallGradeLookup>>(new NullLoggerFactory());
                var dualSpecialismOverallGradeLookupRepository = new GenericRepository<DualSpecialismOverallGradeLookup>(DualSpecialismOverallGradeLookupLogger, DbContext);
                SpecialismResultStrategyFactory = new SpecialismResultStrategyFactory(dualSpecialismOverallGradeLookupRepository);

                OverallGradeLookupLogger = new Logger<GenericRepository<OverallGradeLookup>>(new NullLoggerFactory());
                var overallGradeLookupRepository = new GenericRepository<OverallGradeLookup>(OverallGradeLookupLogger, DbContext);
                OverallGradeStrategyFactory = new OverallGradeStrategyFactory(overallGradeLookupRepository);

                var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(OverallResultCalculationMapper).Assembly));
                Mapper = new Mapper(mapperConfig);

                // Create Service class to test. 
                OverallResultCalculationService = new OverallResultCalculationService(ResultsAndCertificationConfiguration, TlLookupRepository, OverallResultCalculationRepository, AssessmentSeriesRepository, OverallResultRepository, SpecialismResultStrategyFactory, OverallGradeStrategyFactory, Mapper);
            }
        }

        public int GetAcademicYear()
        {
            return AcademicYears.FirstOrDefault(x => DateTime.Today >= x.StartDate && DateTime.Today <= x.EndDate).Year;
        }

        public List<TqRegistrationProfile> SeedRegistrationsData(Dictionary<long, RegistrationPathwayStatus> ulns, TqProvider tqProvider = null, bool isCouplet = false)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedRegistrationData(uln.Key, uln.Value, tqProvider, isCouplet));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null, bool isCouplet = false)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProvider);
            var tqRegistrationSpecialisms = isCouplet ? RegistrationsDataProvider.CreateTqRegistrationSpecialisms(DbContext, tqRegistrationPathway)
                : new List<TqRegistrationSpecialism> { RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism) };

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.Status = status;
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                foreach (var tqRegistrationSpecialism in tqRegistrationSpecialisms)
                {
                    tqRegistrationSpecialism.IsOptedin = true;
                    tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
                }
            }

            DbContext.SaveChanges();
            return profile;
        }

        public List<TqRegistrationProfile> SeedDualSpecialismRegistrationData(IEnumerable<long> ulns)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedDualSpecialismRegistrationData(uln));
            }
            return profiles;
        }

        public List<TqRegistrationProfile> SeedNoSpecialismRegistrationData(IEnumerable<long> ulns)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedNoSpecialismRegistrationData(uln));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedNoSpecialismRegistrationData(long uln)
        {
            var profile = new TqRegistrationProfileBuilder().Build(uln);

            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProvider);

            return profile;
        }

        public TqRegistrationProfile SeedDualSpecialismRegistrationData(long uln)
        {
            var profile = new TqRegistrationProfileBuilder().Build(uln);

            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProvider);
            RegistrationsDataProvider.CreateTqRegistrationSpecialisms(DbContext, tqRegistrationPathway);

            return profile;
        }

        public List<TqPathwayAssessment> GetPathwayAssessmentsDataToProcess(List<TqRegistrationPathway> pathwayRegistrations, bool seedPathwayAssessmentsAsActive = true, bool isHistorical = false, string assessmentSeriesName = "Summer 2021")
        {
            var tqPathwayAssessments = new List<TqPathwayAssessment>();

            foreach (var (pathwayRegistration, index) in pathwayRegistrations.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var pathwayAssessment = new TqPathwayAssessmentBuilder().Build(pathwayRegistration, AssessmentSeries[index]);
                    pathwayAssessment.IsOptedin = false;
                    pathwayAssessment.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqPathwayAssessmentHistorical = PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, pathwayAssessment);
                    tqPathwayAssessments.Add(tqPathwayAssessmentHistorical);
                }

                var assessmentSeries = AssessmentSeries.FirstOrDefault(x => x.ComponentType == ComponentType.Core && x.Name.Equals(assessmentSeriesName, StringComparison.InvariantCultureIgnoreCase));
                var activePathwayAssessment = new TqPathwayAssessmentBuilder().Build(pathwayRegistration, assessmentSeries);
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

        public List<TqPathwayResult> SeedPathwayResultsData(List<TqPathwayResult> pathwayResults, bool saveChanges = true)
        {
            var tqPathwayResult = TqPathwayResultDataProvider.CreateTqPathwayResults(DbContext, pathwayResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqPathwayResult;
        }

        public List<TqSpecialismAssessment> SeedSpecialismAssessmentsData(List<TqSpecialismAssessment> specialismAssessments, bool saveChanges = true)
        {
            var tqSpecialismAssessments = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessments(DbContext, specialismAssessments);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismAssessments;
        }

        public List<TqSpecialismResult> SeedSpecialismResultsData(List<TqSpecialismResult> specialismResults, bool saveChanges = true)
        {
            var tqSpecialismResults = TqSpecialismResultDataProvider.CreateTqSpecialismResults(DbContext, specialismResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismResults;
        }


        public List<OverallResult> SeedOverallResultData(List<TqRegistrationProfile> registrations, List<long> ulnsWithOverallResult, bool saveChanges = true)
        {
            var overallResults = new List<OverallResult>();
            foreach (var ulnOverResult in ulnsWithOverallResult)
            {
                var registration = registrations.FirstOrDefault(reg => reg.UniqueLearnerNumber == ulnOverResult);
                overallResults.Add(OverallResultDataProvider.CreateOverallResult(DbContext, registration.TqRegistrationPathways.FirstOrDefault()));
            }

            if (saveChanges)
                DbContext.SaveChanges();
            return overallResults;
        }

        public IList<TqSpecialismResult> GetSpecialismResultDataToProcess(TqSpecialismAssessment specialismAssessment, bool seedSpecialismResultsAsActive, bool isHistorical, PrsStatus? prsStatus = null, bool isBulkUpload = true)
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
                tqSpecialismResult.IsOptedin = specialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn;
                tqSpecialismResult.EndDate = DateTime.UtcNow;
            }
            else
            {
                tqSpecialismResult.PrsStatus = prsStatus;
            }

            tqSpecialismResults.Add(tqSpecialismResult);
            return tqSpecialismResults;
        }

        public List<TqSpecialismAssessment> GetSpecialismAssessmentsDataToProcess(List<TqRegistrationSpecialism> specialismRegistrations, bool seedSpecialismAssessmentsAsActive = true, bool isHistorical = false, string assessmentSeriesName = "Summer 2022")
        {
            var tqSpecialismAssessments = new List<TqSpecialismAssessment>();

            foreach (var (specialismRegistration, index) in specialismRegistrations.Select((value, i) => (value, i)))
            {
                var assessmentSeries = AssessmentSeries.FirstOrDefault(x => x.Name.Equals(assessmentSeriesName, StringComparison.InvariantCultureIgnoreCase));
                if (isHistorical)
                {
                    // Historical record
                    var specialismAssessment = new TqSpecialismAssessmentBuilder().Build(specialismRegistration, assessmentSeries);
                    specialismAssessment.IsOptedin = false;
                    specialismAssessment.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqSpecialismAssessmentHistorical = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialismAssessment);
                    tqSpecialismAssessments.Add(tqSpecialismAssessmentHistorical);
                }

                var activeSpecialismAssessment = new TqSpecialismAssessmentBuilder().Build(specialismRegistration, assessmentSeries);
                var tqSpecialismAssessment = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, activeSpecialismAssessment);
                if (!seedSpecialismAssessmentsAsActive)
                {
                    tqSpecialismAssessment.IsOptedin = specialismRegistration.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? true : false;
                    tqSpecialismAssessment.EndDate = DateTime.UtcNow;
                }
                tqSpecialismAssessments.Add(tqSpecialismAssessment);

            }
            return tqSpecialismAssessments;
        }

        public List<TqPathwayResult> GetPathwayResultsDataToProcess(List<TqPathwayAssessment> pathwayAssessments, bool seedPathwayResultsAsActive = true, bool isHistorical = false)
        {
            var tqPathwayResults = new List<TqPathwayResult>();

            foreach (var (pathwayAssessment, index) in pathwayAssessments.Select((value, i) => (value, i)))
            {
                var tqresults = GetPathwayResultDataToProcess(pathwayAssessment, seedPathwayResultsAsActive, isHistorical, null, true);
                tqPathwayResults.AddRange(tqresults);
            }
            return tqPathwayResults;
        }

        public List<TqPathwayResult> GetPathwayResultDataToProcess(TqPathwayAssessment pathwayAssessment, bool seedPathwayResultsAsActive = true, bool isHistorical = false, PrsStatus? prsStatus = null, bool isBulkUpload = true)
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
                tqPathwayResult.IsOptedin = pathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn;
                tqPathwayResult.EndDate = DateTime.UtcNow;
            }
            else
            {
                tqPathwayResult.PrsStatus = prsStatus;
            }

            tqPathwayResults.Add(tqPathwayResult);
            return tqPathwayResults;
        }

        public List<TqSpecialismResult> GetSpecialismResultsDataToProcess(List<TqSpecialismAssessment> specialismAssessments, bool seedSpecialismResultsAsActive = true, bool isHistorical = false)
        {
            var tqSpecialismResults = new List<TqSpecialismResult>();

            foreach (var (specialismAssessment, index) in specialismAssessments.Select((value, i) => (value, i)))
            {
                var tqresults = GetSpecialismResultDataToProcess(specialismAssessments, seedSpecialismResultsAsActive, isHistorical, null, true);
                tqSpecialismResults.AddRange(tqresults);
            }
            return tqSpecialismResults;
        }

        public List<TqSpecialismResult> GetSpecialismResultDataToProcess(List<TqSpecialismAssessment> specialismAssessments, bool seedSpecialismResultsAsActive = true, bool isHistorical = false, PrsStatus? prsStatus = null, bool isBulkUpload = true)
        {
            var tqSpecialismResults = new List<TqSpecialismResult>();

            foreach (var (specialismAssessment, index) in specialismAssessments.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var specialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment, SpecialismComponentGrades[index]);
                    specialismResult.IsOptedin = false;
                    specialismResult.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqSpecialismResultHistorical = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, specialismResult);
                    tqSpecialismResults.Add(tqSpecialismResultHistorical);
                }

                var activeSpecialismResult = new TqSpecialismResultBuilder().Build(specialismAssessment, SpecialismComponentGrades[index]);
                var tqSpecialismResult = TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, activeSpecialismResult);
                if (!seedSpecialismResultsAsActive)
                {
                    tqSpecialismResult.IsOptedin = specialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? true : false;
                    tqSpecialismResult.EndDate = DateTime.UtcNow;
                }

                tqSpecialismResults.Add(tqSpecialismResult);
            }
            return tqSpecialismResults;
        }

        public void SeedAssessmentsAndResults(List<TqRegistrationProfile> registrations, List<long> componentWithAssessments, List<long> componentWithResults, string assessmentSeriesName)
        {
            // Pathway Assessments
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();

            foreach (var registration in registrations.Where(x => componentWithAssessments.Contains(x.UniqueLearnerNumber)))
            {
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), assessmentSeriesName: assessmentSeriesName);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                var pathwayAssessmentsWithResults = pathwayAssessments.Where(x => componentWithResults.Contains(x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber)).ToList();
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessmentsWithResults));
            }

            // Specialism Assessments
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();

            foreach (var registration in registrations.Where(x => componentWithAssessments.Contains(x.UniqueLearnerNumber)))
            {
                var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.FirstOrDefault().TqRegistrationSpecialisms.ToList(), assessmentSeriesName: assessmentSeriesName);
                tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);

                var specialismAssessmentsWithResults = specialismAssessments.Where(x => componentWithResults.Contains(x.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber)).ToList();
                tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(specialismAssessmentsWithResults));
            }

            DbContext.SaveChanges();
        }

        public void SeedAssesmentsResults(List<TqRegistrationProfile> registrations, long uln, string[] specialismGradeCodes, string assessmentSeriesName)
        {
            TqRegistrationProfile registration = registrations.Single(x => x.UniqueLearnerNumber == uln);
            TqRegistrationPathway pathway = registration.TqRegistrationPathways.Single();
            ICollection<TqRegistrationSpecialism> specialisms = pathway.TqRegistrationSpecialisms;

            foreach (TqRegistrationSpecialism currentSpecialism in specialisms)
            {
                var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.FirstOrDefault().TqRegistrationSpecialisms.ToList(), assessmentSeriesName: assessmentSeriesName);

                for (int i = 0; i < specialismAssessments.Count; i++)
                {
                    if (specialismGradeCodes[i] != null)
                    {
                        var specialismGrade = TlLookup.SingleOrDefault(s => s.Category == "SpecialismComponentGrade" && s.Value == specialismGradeCodes[i]);

                        var currentSpecialismResult = new TqSpecialismResultBuilder().Build(specialismAssessments[i], specialismGrade);
                        specialismAssessments[i].TqSpecialismResults.Add(currentSpecialismResult);
                    }
                }
            }
        }

        public static void AssertOverallResult(OverallResult actualOverallResult, OverallResult expectedOverallResult)
        {
            actualOverallResult.TqRegistrationPathwayId.Should().Be(expectedOverallResult.TqRegistrationPathwayId);
            actualOverallResult.Details.Should().Be(expectedOverallResult.Details);
            actualOverallResult.ResultAwarded.Should().Be(expectedOverallResult.ResultAwarded);
            actualOverallResult.CalculationStatus.Should().Be(expectedOverallResult.CalculationStatus);
            actualOverallResult.PrintAvailableFrom.Should().Be(expectedOverallResult.PrintAvailableFrom);
            actualOverallResult.PublishDate.Should().Be(expectedOverallResult.PublishDate);
            if (expectedOverallResult.EndDate == null)
            {
                actualOverallResult.IsOptedin.Should().BeTrue();
                actualOverallResult.EndDate.Should().BeNull();
            }
            else
            {
                actualOverallResult.IsOptedin.Should().BeFalse();
                actualOverallResult.EndDate.Should().NotBeNull();
            }

            actualOverallResult.CertificateType.Should().Be(expectedOverallResult.CertificateType);
            actualOverallResult.CertificateStatus.Should().Be(expectedOverallResult.CertificateStatus);
        }

        public enum Provider
        {
            BarsleyCollege = 10000536,
            WalsallCollege = 10007315
        }
    }
}
