using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    [Collection("BulkAssessment")]
    public class When_CompareAndProcessAssessmentsAsync_Called_With_Valid_Assessments : IClassFixture<BulkAssessmentsTextFixture>
    {
        private AssessmentProcessResponse _result;
        private BulkAssessmentsTextFixture _bulkAssessmentsTestFixture;

        public When_CompareAndProcessAssessmentsAsync_Called_With_Valid_Assessments(BulkAssessmentsTextFixture bulkAssessmentsTestFixture)
        {
            // Given
            _bulkAssessmentsTestFixture = bulkAssessmentsTestFixture;
            _bulkAssessmentsTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson);
            _bulkAssessmentsTestFixture.Ulns = new List<long> { 1111111111, 1111111112, 1111111113 };
            _bulkAssessmentsTestFixture.TqRegistrationProfilesData = _bulkAssessmentsTestFixture.SeedRegistrationsData(_bulkAssessmentsTestFixture.Ulns);
            var registrationPathways = _bulkAssessmentsTestFixture.TqRegistrationProfilesData.SelectMany(x => x.TqRegistrationPathways);
            var registrationSpecialisms = registrationPathways.SelectMany(x => x.TqRegistrationSpecialisms);
            _bulkAssessmentsTestFixture.TqPathwayAssessmentsData = _bulkAssessmentsTestFixture.GetPathwayAssessmentsDataToProcess(registrationPathways.ToList());
            _bulkAssessmentsTestFixture.TqSpecialismAssessmentsData = _bulkAssessmentsTestFixture.GetSpecialismAssessmentsDataToProcess(registrationSpecialisms.ToList());
        }

        [Fact(Skip = "Waiting for Devops to setup integration tests")]
        public async Task Then_Expected_Assessments_Are_Created()
        {
            // when
            await _bulkAssessmentsTestFixture.WhenAsync();

            // then
            _result = _bulkAssessmentsTestFixture.Result;
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeTrue();

            foreach(var uln in _bulkAssessmentsTestFixture.Ulns)
            {
                var expectedRegistrationProfile = _bulkAssessmentsTestFixture.TqRegistrationProfilesData.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                var registrationPathway = expectedRegistrationProfile.TqRegistrationPathways.First();
                var expectedPathwayAssessment = _bulkAssessmentsTestFixture.TqPathwayAssessmentsData.FirstOrDefault(p => p.TqRegistrationPathwayId == registrationPathway.Id);

                var actualPathwayAssessment = _bulkAssessmentsTestFixture.DbContext.TqPathwayAssessment.FirstOrDefault(x => x.TqRegistrationPathwayId == registrationPathway.Id && x.IsOptedin && x.EndDate == null);

                // assert registration profile data
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

                // assert registration profile data
                actualSpecialismAssessment.Should().NotBeNull();
                actualSpecialismAssessment.TqRegistrationSpecialismId.Should().Be(expectedSpecialismAssessment.TqRegistrationSpecialismId);
                actualSpecialismAssessment.AssessmentSeriesId.Should().Be(expectedSpecialismAssessment.AssessmentSeriesId);
                actualSpecialismAssessment.IsOptedin.Should().Be(expectedSpecialismAssessment.IsOptedin);
                actualSpecialismAssessment.IsBulkUpload.Should().Be(expectedSpecialismAssessment.IsBulkUpload);
                actualSpecialismAssessment.StartDate.ToShortDateString().Should().Be(expectedSpecialismAssessment.StartDate.ToShortDateString());
                actualSpecialismAssessment.CreatedBy.Should().Be(expectedSpecialismAssessment.CreatedBy);
            }            
        }
    }
}
