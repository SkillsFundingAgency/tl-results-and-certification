using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ResultServiceTests
{
    [Collection("BulkProcessTests")]
    public class When_CompareAndProcessResultsAsync_Called_With_Valid_Results : IClassFixture<BulkResultsTextFixture>
    {
        private ResultProcessResponse _result;
        private BulkResultsTextFixture _bulkResultsTestFixture;

        public When_CompareAndProcessResultsAsync_Called_With_Valid_Results(BulkResultsTextFixture bulkResultsTestFixture)
        {
            // Given
            _bulkResultsTestFixture = bulkResultsTestFixture;
            _bulkResultsTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson);
            _bulkResultsTestFixture.Ulns = new List<long> { 1111111111, 1111111112, 1111111113 };
            _bulkResultsTestFixture.TqRegistrationProfilesData = _bulkResultsTestFixture.SeedRegistrationsData(_bulkResultsTestFixture.Ulns);
            
            var registrationPathways = _bulkResultsTestFixture.TqRegistrationProfilesData.SelectMany(x => x.TqRegistrationPathways);
            
            _bulkResultsTestFixture.TqPathwayAssessmentsData = _bulkResultsTestFixture.SeedPathwayAssessmentsData(registrationPathways.ToList());
            _bulkResultsTestFixture.TqPathwayResultsData = _bulkResultsTestFixture.GetPathwayResultsDataToProcess(_bulkResultsTestFixture.TqPathwayAssessmentsData.ToList());

            _bulkResultsTestFixture.TqSpecialismAssessmentsData = _bulkResultsTestFixture.SeedSpecialismAssessmentData(registrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList());
            _bulkResultsTestFixture.TqSpecialismResultsData = _bulkResultsTestFixture.GetSpecialismResultsDataToProcess(_bulkResultsTestFixture.TqSpecialismAssessmentsData.ToList());

            var pathwayResultIndex = 0;
            foreach (var pathwayResult in _bulkResultsTestFixture.TqPathwayResultsData)
            {
                pathwayResult.Id = pathwayResultIndex - Constants.PathwayResultsStartIndex;
                pathwayResultIndex++;
            }

            var specialismResultIndex = 0;
            foreach (var specialismResult in _bulkResultsTestFixture.TqSpecialismResultsData)
            {
                specialismResult.Id = specialismResultIndex - Constants.SpecialismResultsStartIndex;
                specialismResultIndex++;
            }
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Created()
        {
            // when
            await _bulkResultsTestFixture.WhenAsync();

            // then
            _result = _bulkResultsTestFixture.Result;
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeTrue();

            foreach (var uln in _bulkResultsTestFixture.Ulns)
            {
                var expectedRegistrationProfile = _bulkResultsTestFixture.TqRegistrationProfilesData.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                var registrationPathway = expectedRegistrationProfile.TqRegistrationPathways.First();
                var registeredSpecialisms = registrationPathway.TqRegistrationSpecialisms;

                var pathwayAssessment = _bulkResultsTestFixture.TqPathwayAssessmentsData.FirstOrDefault(p => p.TqRegistrationPathwayId == registrationPathway.Id);

                var expectedPathwayResult = _bulkResultsTestFixture.TqPathwayResultsData.FirstOrDefault(r => r.TqPathwayAssessmentId == pathwayAssessment.Id);

                var actualPathwayResult = _bulkResultsTestFixture.DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessmentId == pathwayAssessment.Id && x.IsOptedin && x.EndDate == null);

                // assert registration profile data
                actualPathwayResult.Should().NotBeNull();
                actualPathwayResult.TqPathwayAssessmentId.Should().Be(expectedPathwayResult.TqPathwayAssessmentId);
                actualPathwayResult.TlLookupId.Should().Be(expectedPathwayResult.TlLookupId);
                actualPathwayResult.IsOptedin.Should().Be(expectedPathwayResult.IsOptedin);
                actualPathwayResult.IsBulkUpload.Should().Be(expectedPathwayResult.IsBulkUpload);
                actualPathwayResult.StartDate.ToShortDateString().Should().Be(expectedPathwayResult.StartDate.ToShortDateString());
                actualPathwayResult.CreatedBy.Should().Be(expectedPathwayResult.CreatedBy);

                foreach (var registeredSpecialism in registeredSpecialisms)
                {
                    var specialismAssessment = _bulkResultsTestFixture.TqSpecialismAssessmentsData.FirstOrDefault(p => p.TqRegistrationSpecialismId == registeredSpecialism.Id);

                    var expectedSpecialismResult = _bulkResultsTestFixture.TqSpecialismResultsData.FirstOrDefault(r => r.TqSpecialismAssessmentId == specialismAssessment.Id);

                    var actualSpecialismResult = _bulkResultsTestFixture.DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessmentId == specialismAssessment.Id && x.IsOptedin && x.EndDate == null);

                    // assert specialism result data
                    actualSpecialismResult.Should().NotBeNull();
                    actualSpecialismResult.TqSpecialismAssessmentId.Should().Be(expectedSpecialismResult.TqSpecialismAssessmentId);
                    actualSpecialismResult.TlLookupId.Should().Be(expectedSpecialismResult.TlLookupId);
                    actualSpecialismResult.IsOptedin.Should().Be(expectedSpecialismResult.IsOptedin);
                    actualSpecialismResult.IsBulkUpload.Should().Be(expectedSpecialismResult.IsBulkUpload);
                    actualSpecialismResult.StartDate.ToShortDateString().Should().Be(expectedSpecialismResult.StartDate.ToShortDateString());
                    actualSpecialismResult.CreatedBy.Should().Be(expectedSpecialismResult.CreatedBy);
                }
            }
        }
    }
}