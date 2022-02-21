using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    [Collection("BulkProcessTests")]
    public class When_CompareAndProcessAssessmentsAsync_Called_With_Valid_Assessments : IClassFixture<BulkAssessmentsTextFixture>
    {
        private AssessmentProcessResponse _result;
        private BulkAssessmentsTextFixture _bulkAssessmentsTestFixture;
        private long _removeAssessmentEntryUln;
        private int _removedExistingPathwayAssessmentSeriesId;
        private int _removedExistingSpecialismAssessmentSeriesId;

        public When_CompareAndProcessAssessmentsAsync_Called_With_Valid_Assessments(BulkAssessmentsTextFixture bulkAssessmentsTestFixture)
        {
            // Given
            _bulkAssessmentsTestFixture = bulkAssessmentsTestFixture;
            _bulkAssessmentsTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson);

            // Seed Profile, Assessment entries for Pathway and Specialism to remove assessment entries
            _removeAssessmentEntryUln = 1111111114;
            var removeAssessmentEntryProfile = _bulkAssessmentsTestFixture.SeedRegistrationData(_removeAssessmentEntryUln);
            var removeAssessmentEntryPathway = _bulkAssessmentsTestFixture.SeedPathwayAssessmentData(removeAssessmentEntryProfile);
            var removeAssessmentEntrySpecialism = _bulkAssessmentsTestFixture.SeedSpecialismAssessmentData(removeAssessmentEntryProfile.TqRegistrationPathways.SelectMany(x => x.TqRegistrationSpecialisms).FirstOrDefault());
            
            // Store saved assessment series id for pathway and specialisms
            _removedExistingPathwayAssessmentSeriesId = removeAssessmentEntryPathway.AssessmentSeriesId;
            _removedExistingSpecialismAssessmentSeriesId = removeAssessmentEntrySpecialism.AssessmentSeriesId;

            // Seed Profile data
            _bulkAssessmentsTestFixture.Ulns = new List<long> { 1111111111, 1111111112, 1111111113 };
            _bulkAssessmentsTestFixture.TqRegistrationProfilesData = _bulkAssessmentsTestFixture.SeedRegistrationsData(_bulkAssessmentsTestFixture.Ulns);

            // Second Cohort Uln for specialism assessment to register from 1st year
            var secondCohortUln = 1111111113;
            _bulkAssessmentsTestFixture.TqRegistrationProfilesData.FirstOrDefault(p => p.UniqueLearnerNumber == secondCohortUln).TqRegistrationPathways.FirstOrDefault().AcademicYear = 2021;

            var registrationPathways = _bulkAssessmentsTestFixture.TqRegistrationProfilesData.SelectMany(x => x.TqRegistrationPathways);
            var registrationSpecialisms = registrationPathways.SelectMany(x => x.TqRegistrationSpecialisms);

            // Prepare assessment entries data for pathways and specialism for above Unls
            _bulkAssessmentsTestFixture.TqPathwayAssessmentsData = _bulkAssessmentsTestFixture.GetPathwayAssessmentsDataToProcess(registrationPathways.ToList());
            _bulkAssessmentsTestFixture.TqSpecialismAssessmentsData = _bulkAssessmentsTestFixture.GetSpecialismAssessmentsDataToProcess(registrationSpecialisms.ToList());

            var pathwayAssessmentIndex = 0;
            foreach (var pathwayAssessment in _bulkAssessmentsTestFixture.TqPathwayAssessmentsData)
            {
                pathwayAssessment.Id = pathwayAssessmentIndex - Constants.PathwayAssessmentsStartIndex;
                pathwayAssessmentIndex++;
            }

            _bulkAssessmentsTestFixture.TqRegistrationProfilesData.Add(removeAssessmentEntryProfile);

            var specialismAssessmentIndex = 0;
            foreach (var specialismAssessment in _bulkAssessmentsTestFixture.TqSpecialismAssessmentsData)
            {
                specialismAssessment.Id = specialismAssessmentIndex - Constants.SpecialismAssessmentsStartIndex;
                specialismAssessmentIndex++;
            }

            var pathwayAssessmentToRemove = _bulkAssessmentsTestFixture.GetPathwayAssessmentsDataToProcess(removeAssessmentEntryProfile.TqRegistrationPathways.ToList());
            pathwayAssessmentToRemove.ForEach(p => p.AssessmentSeriesId = 0);

            foreach (var pathwayAssessmentEntry in pathwayAssessmentToRemove)
                _bulkAssessmentsTestFixture.TqPathwayAssessmentsData.Add(pathwayAssessmentEntry);

            var specialismAssessmentToRemove = _bulkAssessmentsTestFixture.GetSpecialismAssessmentsDataToProcess(removeAssessmentEntryProfile.TqRegistrationPathways.SelectMany(x => x.TqRegistrationSpecialisms).ToList());
            specialismAssessmentToRemove.ForEach(p => p.AssessmentSeriesId = 0);

            foreach (var specialismAssessmentEntry in specialismAssessmentToRemove)
            _bulkAssessmentsTestFixture.TqSpecialismAssessmentsData.Add(specialismAssessmentEntry);
        }

        [Fact]
        public async Task Then_Expected_Assessments_Are_Created()
        {
            // when
            await _bulkAssessmentsTestFixture.WhenAsync();

            // then
            _result = _bulkAssessmentsTestFixture.Result;
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeTrue();

            // Assert Add Assessment entries
            foreach (var uln in _bulkAssessmentsTestFixture.Ulns)
            {
                var expectedRegistrationProfile = _bulkAssessmentsTestFixture.TqRegistrationProfilesData.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                var registrationPathway = expectedRegistrationProfile.TqRegistrationPathways.First();
                var expectedPathwayAssessment = _bulkAssessmentsTestFixture.TqPathwayAssessmentsData.FirstOrDefault(p => p.TqRegistrationPathwayId == registrationPathway.Id);

                var actualPathwayAssessment = _bulkAssessmentsTestFixture.DbContext.TqPathwayAssessment.FirstOrDefault(x => x.TqRegistrationPathwayId == registrationPathway.Id && x.IsOptedin && x.EndDate == null);

                // assert pathway assessment data
                actualPathwayAssessment.Should().NotBeNull();
                actualPathwayAssessment.TqRegistrationPathwayId.Should().Be(expectedPathwayAssessment.TqRegistrationPathwayId);
                actualPathwayAssessment.AssessmentSeriesId.Should().Be(expectedPathwayAssessment.AssessmentSeriesId);
                actualPathwayAssessment.IsOptedin.Should().Be(expectedPathwayAssessment.IsOptedin);
                actualPathwayAssessment.IsBulkUpload.Should().Be(expectedPathwayAssessment.IsBulkUpload);
                actualPathwayAssessment.StartDate.ToShortDateString().Should().Be(expectedPathwayAssessment.StartDate.ToShortDateString());
                actualPathwayAssessment.CreatedBy.Should().Be(expectedPathwayAssessment.CreatedBy);

                var registrationSpecialism = registrationPathway.TqRegistrationSpecialisms.First();
                var expectedSpecialismAssessment = _bulkAssessmentsTestFixture.TqSpecialismAssessmentsData.FirstOrDefault(p => p.TqRegistrationSpecialismId == registrationSpecialism.Id);
                var actualSpecialismAssessment = _bulkAssessmentsTestFixture.DbContext.TqSpecialismAssessment.FirstOrDefault(x => x.TqRegistrationSpecialismId == registrationSpecialism.Id && x.IsOptedin && x.EndDate == null);

                // assert specialism assessment data
                actualSpecialismAssessment.Should().NotBeNull();
                actualSpecialismAssessment.TqRegistrationSpecialismId.Should().Be(expectedSpecialismAssessment.TqRegistrationSpecialismId);
                actualSpecialismAssessment.AssessmentSeriesId.Should().Be(expectedSpecialismAssessment.AssessmentSeriesId);
                actualSpecialismAssessment.IsOptedin.Should().Be(expectedSpecialismAssessment.IsOptedin);
                actualSpecialismAssessment.IsBulkUpload.Should().Be(expectedSpecialismAssessment.IsBulkUpload);
                actualSpecialismAssessment.StartDate.ToShortDateString().Should().Be(expectedSpecialismAssessment.StartDate.ToShortDateString());
                actualSpecialismAssessment.CreatedBy.Should().Be(expectedSpecialismAssessment.CreatedBy);
            }

            // Assert Remove Assessment entries

            var expectedRemoveRegistrationProfile = _bulkAssessmentsTestFixture.TqRegistrationProfilesData.FirstOrDefault(p => p.UniqueLearnerNumber == _removeAssessmentEntryUln);
            
            var removeRegistrationPathway = expectedRemoveRegistrationProfile.TqRegistrationPathways.First();
            var expectedRemovedPathwayAssessment = _bulkAssessmentsTestFixture.TqPathwayAssessmentsData.FirstOrDefault(p => p.TqRegistrationPathwayId == removeRegistrationPathway.Id);
            var actualRemovedPathwayAssessment = _bulkAssessmentsTestFixture.DbContext.TqPathwayAssessment.FirstOrDefault(x => x.TqRegistrationPathwayId == removeRegistrationPathway.Id && !x.IsOptedin && x.EndDate != null);

            // assert pathway assessment data
            actualRemovedPathwayAssessment.Should().NotBeNull();
            actualRemovedPathwayAssessment.TqRegistrationPathwayId.Should().Be(expectedRemovedPathwayAssessment.TqRegistrationPathwayId);
            actualRemovedPathwayAssessment.AssessmentSeriesId.Should().Be(_removedExistingPathwayAssessmentSeriesId);
            actualRemovedPathwayAssessment.IsOptedin.Should().BeFalse();
            actualRemovedPathwayAssessment.IsBulkUpload.Should().Be(expectedRemovedPathwayAssessment.IsBulkUpload);
            actualRemovedPathwayAssessment.StartDate.ToShortDateString().Should().Be(expectedRemovedPathwayAssessment.StartDate.ToShortDateString());
            actualRemovedPathwayAssessment.EndDate.Should().NotBeNull();
            actualRemovedPathwayAssessment.ModifiedBy.Should().Be(expectedRemovedPathwayAssessment.CreatedBy);
            actualRemovedPathwayAssessment.ModifiedOn.Should().NotBeNull();

            var removeRegistrationSpecialism = removeRegistrationPathway.TqRegistrationSpecialisms.First();
            var expectedRemovedSpecialismAssessment = _bulkAssessmentsTestFixture.TqSpecialismAssessmentsData.FirstOrDefault(p => p.TqRegistrationSpecialismId == removeRegistrationSpecialism.Id);
            var actualRemovedSpecialismAssessment = _bulkAssessmentsTestFixture.DbContext.TqSpecialismAssessment.FirstOrDefault(x => x.TqRegistrationSpecialismId == removeRegistrationSpecialism.Id && !x.IsOptedin && x.EndDate != null);

            // assert specialism assessment data
            actualRemovedSpecialismAssessment.Should().NotBeNull();
            actualRemovedSpecialismAssessment.TqRegistrationSpecialismId.Should().Be(expectedRemovedSpecialismAssessment.TqRegistrationSpecialismId);
            actualRemovedSpecialismAssessment.AssessmentSeriesId.Should().Be(_removedExistingSpecialismAssessmentSeriesId);
            actualRemovedSpecialismAssessment.IsOptedin.Should().BeFalse();
            actualRemovedSpecialismAssessment.IsBulkUpload.Should().Be(expectedRemovedSpecialismAssessment.IsBulkUpload);
            actualRemovedSpecialismAssessment.StartDate.ToShortDateString().Should().Be(expectedRemovedSpecialismAssessment.StartDate.ToShortDateString());
            actualRemovedSpecialismAssessment.EndDate.Should().NotBeNull();
            actualRemovedSpecialismAssessment.ModifiedBy.Should().Be(expectedRemovedSpecialismAssessment.CreatedBy);
            actualRemovedSpecialismAssessment.ModifiedOn.Should().NotBeNull();

        }
    }
}
