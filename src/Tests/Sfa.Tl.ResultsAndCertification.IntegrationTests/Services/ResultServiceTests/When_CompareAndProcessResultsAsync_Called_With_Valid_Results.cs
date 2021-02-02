using FluentAssertions;
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
            }
        }
    }
}