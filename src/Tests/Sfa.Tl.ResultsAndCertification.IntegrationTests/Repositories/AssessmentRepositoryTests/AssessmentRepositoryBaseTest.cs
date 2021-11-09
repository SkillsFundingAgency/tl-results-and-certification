using FluentAssertions;
using Microsoft.Extensions.Logging;
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

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AssessmentRepositoryTests
{
    public abstract class AssessmentRepositoryBaseTest : BaseTest<TqPathwayAssessment>
    {
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
        protected IList<TqProvider> TqProviders;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<AcademicYear> AcademicYears;

        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected IAssessmentRepository AssessmentRepository;
        protected ILogger<AssessmentRepository> AssessmentRepositoryLogger;
        protected IRepository<AssessmentSeries> AssessmentSeriesRepository;
        protected ILogger<GenericRepository<AssessmentSeries>> AssessmentSeriesRepositoryLogger;

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
            AcademicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);

            DbContext.SaveChangesAsync();
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
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProvider);
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            DbContext.SaveChanges();
            return profile;
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

        public TqRegistrationProfile SeedRegistrationDataByStatus(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProvider);
            tqRegistrationPathway.Status = status;

            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                tqRegistrationSpecialism.IsOptedin = true;
                tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
            }

            DbContext.SaveChanges();
            return profile;
        }

        public List<TqPathwayAssessment> SeedPathwayAssessmentsData(List<TqPathwayAssessment> pathwayAssessments, bool saveChanges = true)
        {            
            var tqPathwayAssessment = PathwayAssessmentDataProvider.CreateTqPathwayAssessments(DbContext, pathwayAssessments);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqPathwayAssessment;
        }

        public List<TqSpecialismAssessment> SeedSpecialismAssessmentsData(List<TqSpecialismAssessment> specialismAssessments, bool saveChanges = true)
        {
            var tqSpecialismAssessments = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessments(DbContext, specialismAssessments);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismAssessments;
        }

        public List<TqPathwayResult> SeedPathwayResultsData(List<TqPathwayResult> pathwayResults, bool saveChanges = true)
        {
            var tqPathwayResult = TqPathwayResultDataProvider.CreateTqPathwayResults(DbContext, pathwayResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqPathwayResult;
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

                var assessmentSeries = AssessmentSeries.FirstOrDefault(x => x.Name.Equals(assessmentSeriesName, StringComparison.InvariantCultureIgnoreCase));
                var activePathwayAssessment = new TqPathwayAssessmentBuilder().Build(pathwayRegistration, assessmentSeries);
                var tqPathwayAssessment = PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, activePathwayAssessment);
                if (!seedPathwayAssessmentsAsActive)
                {
                    tqPathwayAssessment.IsOptedin = pathwayRegistration.Status == Common.Enum.RegistrationPathwayStatus.Withdrawn ? true : false;
                    tqPathwayAssessment.EndDate = DateTime.UtcNow;
                }

                tqPathwayAssessments.Add(tqPathwayAssessment);
            }
            return tqPathwayAssessments;
        }

        public List<TqSpecialismAssessment> GetSpecialismAssessmentsDataToProcess(List<TqRegistrationSpecialism> specialismRegistrations, bool seedSpecialismAssessmentsAsActive = true, bool isHistorical = false)
        {
            var tqSpecialismAssessments = new List<TqSpecialismAssessment>();

            foreach (var (specialismRegistration, index) in specialismRegistrations.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var specialismAssessment = new TqSpecialismAssessmentBuilder().Build(specialismRegistration, AssessmentSeries[index]);
                    specialismAssessment.IsOptedin = false;
                    specialismAssessment.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqSpecialismAssessmentHistorical = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, specialismAssessment);
                    tqSpecialismAssessments.Add(tqSpecialismAssessmentHistorical);
                }

                var activeSpecialismAssessment = new TqSpecialismAssessmentBuilder().Build(specialismRegistration, AssessmentSeries[index]);
                var tqSpecialismAssessment = SpecialismAssessmentDataProvider.CreateTqSpecialismAssessment(DbContext, activeSpecialismAssessment);
                if (!seedSpecialismAssessmentsAsActive)
                {
                    tqSpecialismAssessment.IsOptedin = specialismRegistration.TqRegistrationPathway.Status == Common.Enum.RegistrationPathwayStatus.Withdrawn ? true : false;
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

        public List<TqPathwayAssessment> SeedAssessmentsAndResults(List<TqRegistrationProfile> registrations, List<long> pathwaysWithAssessments, List<long> pathwaysWithResults, string assessmentSeriesName)
        {
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();

            foreach (var registration in registrations.Where(x => pathwaysWithAssessments.Contains(x.UniqueLearnerNumber)))
            {
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), assessmentSeriesName: assessmentSeriesName);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                var pathwayAssessmentsWithResults = pathwayAssessments.Where(x => pathwaysWithResults.Contains(x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber)).ToList();
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessmentsWithResults));
            }

            DbContext.SaveChanges();
            return tqPathwayAssessmentsSeedData;
        }

        public int GetAcademicYear()
        {
            return AcademicYears.FirstOrDefault(x => DateTime.Today >= x.StartDate && DateTime.Today <= x.EndDate).Year;
        }

        #region Assert Methods

        public void AssertSpecialismAssessment(TqSpecialismAssessment actualSpecialismAssessment, TqSpecialismAssessment expectedSpecialismAssessment)
        {
            actualSpecialismAssessment.TqRegistrationSpecialismId.Should().Be(expectedSpecialismAssessment.TqRegistrationSpecialismId);
            actualSpecialismAssessment.AssessmentSeriesId.Should().Be(expectedSpecialismAssessment.AssessmentSeriesId);
            actualSpecialismAssessment.IsOptedin.Should().Be(expectedSpecialismAssessment.IsOptedin);
            actualSpecialismAssessment.IsBulkUpload.Should().Be(expectedSpecialismAssessment.IsBulkUpload);
            actualSpecialismAssessment.StartDate.ToShortDateString().Should().Be(expectedSpecialismAssessment.StartDate.ToShortDateString());
            if (expectedSpecialismAssessment.EndDate != null)
                actualSpecialismAssessment.EndDate.Value.ToShortDateString().Should().Be(expectedSpecialismAssessment.EndDate.Value.ToShortDateString());
            else
                actualSpecialismAssessment.EndDate.Should().BeNull();
            actualSpecialismAssessment.CreatedBy.Should().Be(expectedSpecialismAssessment.CreatedBy);
        }

        public void AssertPathwayAssessment(TqPathwayAssessment actualPathwayAssessment, TqPathwayAssessment expectedPathwayAssessment)
        {
            actualPathwayAssessment.TqRegistrationPathwayId.Should().Be(expectedPathwayAssessment.TqRegistrationPathwayId);
            actualPathwayAssessment.AssessmentSeriesId.Should().Be(expectedPathwayAssessment.AssessmentSeriesId);
            actualPathwayAssessment.IsOptedin.Should().Be(expectedPathwayAssessment.IsOptedin);
            actualPathwayAssessment.IsBulkUpload.Should().Be(actualPathwayAssessment.IsBulkUpload);
            actualPathwayAssessment.StartDate.ToShortDateString().Should().Be(expectedPathwayAssessment.StartDate.ToShortDateString());
            if (expectedPathwayAssessment.EndDate != null)
                actualPathwayAssessment.EndDate.Value.ToShortDateString().Should().Be(expectedPathwayAssessment.EndDate.Value.ToShortDateString());
            else
                actualPathwayAssessment.EndDate.Should().BeNull();
            actualPathwayAssessment.CreatedBy.Should().Be(expectedPathwayAssessment.CreatedBy);
        }

        public void AssertPathwayResult(TqPathwayResult actualPathwayResult, TqPathwayResult expectedPathwayResult)
        {
            if (expectedPathwayResult == null)
            {
                actualPathwayResult.Should().BeNull();
                return;
            }

            actualPathwayResult.TqPathwayAssessmentId.Should().Be(expectedPathwayResult.TqPathwayAssessmentId);
            actualPathwayResult.TlLookupId.Should().Be(expectedPathwayResult.TlLookupId);
            actualPathwayResult.IsOptedin.Should().Be(expectedPathwayResult.IsOptedin);
            actualPathwayResult.IsBulkUpload.Should().Be(expectedPathwayResult.IsBulkUpload);
            actualPathwayResult.StartDate.Should().Be(expectedPathwayResult.StartDate);
            if (expectedPathwayResult.EndDate != null)
                actualPathwayResult.EndDate.Value.ToShortDateString().Should().Be(expectedPathwayResult.EndDate.Value.ToShortDateString());
            else
                actualPathwayResult.EndDate.Should().BeNull();

            actualPathwayResult.CreatedBy.Should().Be(expectedPathwayResult.CreatedBy);
            actualPathwayResult.PrsStatus.Should().Be(expectedPathwayResult.PrsStatus);
        }

        public void AssertIndustryPlacement(IndustryPlacement actualIndustryPlacement, IndustryPlacement expectedIndustryPlacement)
        {
            if (expectedIndustryPlacement == null)
            {
                actualIndustryPlacement.Should().BeNull();
                return;
            }

            actualIndustryPlacement.TqRegistrationPathwayId.Should().Be(expectedIndustryPlacement.TqRegistrationPathwayId);
            actualIndustryPlacement.Status.Should().Be(expectedIndustryPlacement.Status);
            actualIndustryPlacement.CreatedBy.Should().Be(expectedIndustryPlacement.CreatedBy);
        }

        #endregion
    }
}
