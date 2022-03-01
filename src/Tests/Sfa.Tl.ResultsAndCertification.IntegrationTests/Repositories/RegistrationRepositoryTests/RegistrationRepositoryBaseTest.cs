using FluentAssertions;
using Microsoft.Extensions.Logging;
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

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.RegistrationRepositoryTests
{
    public abstract class RegistrationRepositoryBaseTest : BaseTest<TqRegistrationPathway>
    {
        // Seed objects
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TlProvider TlProvider;
        protected TqProvider TqProvider;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<TlLookup> SpecialismComponentGrades;
        protected IList<TqProvider> TqProviders;

        // Dependencies.
        protected IRegistrationRepository RegistrationRepository;
        protected ILogger<RegistrationRepository> RegistrationRepositoryLogger;

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

        public TqRegistrationProfile SeedRegistrationDataByStatus(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
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

        public List<TqSpecialismResult> SeedSpecialismResultsData(List<TqSpecialismResult> specialismResults, bool saveChanges = true)
        {
            var tqSpecialismResults = TqSpecialismResultDataProvider.CreateTqSpecialismResults(DbContext, specialismResults);
            if (saveChanges)
                DbContext.SaveChanges();

            return tqSpecialismResults;
        }

        public List<TqPathwayAssessment> GetPathwayAssessmentsDataToProcess(List<TqRegistrationPathway> pathwayRegistrations, bool seedPathwayAssessmentsAsActive = true, bool isHistorical = false)
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

                var activePathwayAssessment = new TqPathwayAssessmentBuilder().Build(pathwayRegistration, AssessmentSeries[index]);
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

        public List<TqPathwayResult> GetPathwayResultsDataToProcess(List<TqPathwayAssessment> pathwayAssessments, bool seedPathwayResultsAsActive = true, bool isHistorical = false)
        {
            var tqPathwayResults = new List<TqPathwayResult>();

            foreach (var (pathwayAssessment, index) in pathwayAssessments.Select((value, i) => (value, i)))
            {
                if (isHistorical)
                {
                    // Historical record
                    var pathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment, PathwayComponentGrades[index]);
                    pathwayResult.IsOptedin = false;
                    pathwayResult.EndDate = DateTime.UtcNow.AddDays(-1);

                    var tqPathwayResultHistorical = TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, pathwayResult);
                    tqPathwayResults.Add(tqPathwayResultHistorical);
                }

                var activePathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessment, PathwayComponentGrades[index]);
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

        public List<TqSpecialismResult> GetSpecialismResultsDataToProcess(List<TqSpecialismAssessment> specialismAssessments, bool seedSpecialismResultsAsActive = true, bool isHistorical = false)
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

        public void AssertRegistrationProfile(TqRegistrationProfile actualProfile, TqRegistrationProfile expectedProfile)
        {
            if (expectedProfile != null)
                actualProfile.Should().NotBeNull();

            actualProfile.UniqueLearnerNumber.Should().Be(expectedProfile.UniqueLearnerNumber);
            actualProfile.Firstname.Should().Be(expectedProfile.Firstname);
            actualProfile.Lastname.Should().Be(expectedProfile.Lastname);
            actualProfile.DateofBirth.Should().Be(expectedProfile.DateofBirth);
            actualProfile.Gender.Should().Be(expectedProfile.Gender);
            actualProfile.IsLearnerVerified.Should().Be(expectedProfile.IsLearnerVerified);
            actualProfile.IsEnglishAndMathsAchieved.Should().Be(expectedProfile.IsEnglishAndMathsAchieved);
            actualProfile.IsSendLearner.Should().Be(expectedProfile.IsSendLearner);
            actualProfile.IsRcFeed.Should().Be(expectedProfile.IsRcFeed);
        }

        public static void AssertRegistrationPathway(TqRegistrationPathway actualPathway, TqRegistrationPathway expectedPathway, bool assertStatus = true)
        {
            actualPathway.Should().NotBeNull();
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
            actualPathwayResult.TqPathwayAssessmentId.Should().Be(expectedPathwayResult.TqPathwayAssessmentId);
            actualPathwayResult.TlLookupId.Should().Be(expectedPathwayResult.TlLookupId);
            actualPathwayResult.IsOptedin.Should().Be(expectedPathwayResult.IsOptedin);
            actualPathwayResult.IsBulkUpload.Should().Be(expectedPathwayResult.IsBulkUpload);
            actualPathwayResult.StartDate.Should().Be(expectedPathwayResult.StartDate);
            actualPathwayResult.CreatedBy.Should().Be(expectedPathwayResult.CreatedBy);
            if (expectedPathwayResult.EndDate != null)
                actualPathwayResult.EndDate.Value.ToShortDateString().Should().Be(expectedPathwayResult.EndDate.Value.ToShortDateString());
            else
                actualPathwayResult.EndDate.Should().BeNull();            
        }

        public void AssertSpecialismAssessment(IList<TqSpecialismAssessment> actualSpecialismAssessment, IList<TqSpecialismAssessment> expectedSpecialismAssessment)
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

        public void AssertSpecialismResults(IList<TqSpecialismResult> actualSpecialismResults, IList<TqSpecialismResult> expectedSpecialismResults)
        {
            actualSpecialismResults.Should().NotBeEmpty();
            actualSpecialismResults.Should().HaveSameCount(expectedSpecialismResults);

            actualSpecialismResults.ToList().ForEach(actualResult =>
            {
                var expectedResult = expectedSpecialismResults.FirstOrDefault(x => x.Id == actualResult.Id);
                expectedResult.Should().NotBeNull();

                expectedResult.TqSpecialismAssessmentId.Should().Be(expectedResult.TqSpecialismAssessmentId);
                expectedResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
                expectedResult.IsOptedin.Should().Be(expectedResult.IsOptedin);
                expectedResult.IsBulkUpload.Should().Be(expectedResult.IsBulkUpload);
                expectedResult.StartDate.Should().Be(expectedResult.StartDate);
                expectedResult.CreatedBy.Should().Be(expectedResult.CreatedBy);
                if (expectedResult.EndDate != null)
                    actualResult.EndDate.Value.ToShortDateString().Should().Be(expectedResult.EndDate.Value.ToShortDateString());
                else
                    actualResult.EndDate.Should().BeNull();
            });
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
    }
}
