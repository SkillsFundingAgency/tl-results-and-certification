using FluentAssertions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
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

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public abstract class OverallResultCalculationServiceBaseTest : BaseTest<OverallResult>
    {
        // Seed variables
        protected long AoUkprn = 10011881;
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
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<TlLookup> SpecialismComponentGrades;
        protected IList<AcademicYear> AcademicYears;
        protected IList<OverallGradeLookup> OverallGradeLookup;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected OverallResultCalculationService OverallResultCalculationService;

        protected IOverallResultCalculationRepository OverallResultCalculationRepository;

        protected IRepository<TlLookup> TlLookupRepository;
        protected ILogger<GenericRepository<TlLookup>> TlLookupRepositoryLogger;
        protected IRepository<OverallGradeLookup> OverallGradeLookupRepository;
        protected ILogger<GenericRepository<OverallGradeLookup>> OverallGradeLookupLogger;
        protected IRepository<AssessmentSeries> AssessmentSeriesRepository;
        protected ILogger<GenericRepository<AssessmentSeries>> AssessmentSeriesLogger;

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
            TlLookup = TlLookupDataProvider.CreateTlLookupList(DbContext, null, true);
            PathwayComponentGrades = TlLookup.Where(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
            SpecialismComponentGrades = TlLookupDataProvider.CreateSpecialismGradeTlLookupList(DbContext, null, true);
            AcademicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            OverallGradeLookup = OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, null, true);
            DbContext.SaveChanges();
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
    }
}
