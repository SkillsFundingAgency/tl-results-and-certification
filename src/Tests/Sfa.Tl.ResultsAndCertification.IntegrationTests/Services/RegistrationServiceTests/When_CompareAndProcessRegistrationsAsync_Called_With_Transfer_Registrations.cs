using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    [Collection("BulkProcessTests")]
    public class When_CompareAndProcessRegistrationsAsync_Called_With_Transfer_Registrations : IClassFixture<BulkRegistrationsTextFixture>
    {
        private RegistrationProcessResponse _result;
        private BulkRegistrationsTextFixture _bulkRegistrationTestFixture;

        public When_CompareAndProcessRegistrationsAsync_Called_With_Transfer_Registrations(BulkRegistrationsTextFixture bulkRegistrationTestFixture)
        {
            _bulkRegistrationTestFixture = bulkRegistrationTestFixture;

            // Given
            _bulkRegistrationTestFixture.Uln = 1111111111;
            _bulkRegistrationTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson);
            var barnsleyCollegeTqProvider = _bulkRegistrationTestFixture.TqProviders.FirstOrDefault(p => p.TlProvider.UkPrn == 10000536);
            var walsallCollegeTqProvider = _bulkRegistrationTestFixture.TqProviders.FirstOrDefault(p => p.TlProvider.UkPrn == 10007315);

            var seededTqRegistrationProfile = _bulkRegistrationTestFixture.SeedRegistrationData(_bulkRegistrationTestFixture.Uln, barnsleyCollegeTqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var seededRegistrationPathways = seededTqRegistrationProfile.TqRegistrationPathways.ToList();
            tqPathwayAssessmentsSeedData.AddRange(_bulkRegistrationTestFixture.GetPathwayAssessmentsDataToProcess(seededRegistrationPathways));
            var _pathwayAssessments = _bulkRegistrationTestFixture.SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            // Results seed
            var results = _bulkRegistrationTestFixture.GetPathwayResultsDataToProcess(_pathwayAssessments);
            var _pathwayResults = _bulkRegistrationTestFixture.SeedPathwayResultsData(results);

            // Input param
            var registrationDataToProcess = _bulkRegistrationTestFixture.GetRegistrationsDataToProcess(_bulkRegistrationTestFixture.Uln, walsallCollegeTqProvider);
            _bulkRegistrationTestFixture.TqRegistrationProfilesData = new List<TqRegistrationProfile> { registrationDataToProcess };
        }

        [Fact]
        public async Task Then_Expected_Registrations_Are_Amended()
        {
            // when
            await _bulkRegistrationTestFixture.WhenAsync();

            // then
            _result = _bulkRegistrationTestFixture.Result;
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeTrue();
            _result.BulkUploadStats.Should().NotBeNull();
            _result.BulkUploadStats.TotalRecordsCount.Should().Be(_bulkRegistrationTestFixture.TqRegistrationProfilesData.Count);
            _result.BulkUploadStats.NewRecordsCount.Should().Be(0);
            _result.BulkUploadStats.AmendedRecordsCount.Should().Be(1);
            _result.BulkUploadStats.UnchangedRecordsCount.Should().Be(0);
            _result.ValidationErrors.Should().BeNullOrEmpty();

            var expectedRegistrationProfile = _bulkRegistrationTestFixture.TqRegistrationProfilesData.FirstOrDefault(p => p.UniqueLearnerNumber == _bulkRegistrationTestFixture.Uln);

            var actualRegistrationProfile = _bulkRegistrationTestFixture.DbContext.TqRegistrationProfile.AsNoTracking().Where(x => x.UniqueLearnerNumber == _bulkRegistrationTestFixture.Uln)
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                           .ThenInclude(x => x.TqRegistrationSpecialisms)
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                            .ThenInclude(x => x.TqPathwayAssessments)
                                                                                                                                .ThenInclude(x => x.TqPathwayResults)
                                                                                                                       .FirstOrDefault();
            // assert registration profile data
            actualRegistrationProfile.Should().NotBeNull();
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);
            actualRegistrationProfile.Firstname.Should().Be(expectedRegistrationProfile.Firstname);
            actualRegistrationProfile.Lastname.Should().Be(expectedRegistrationProfile.Lastname);
            actualRegistrationProfile.DateofBirth.Should().Be(expectedRegistrationProfile.DateofBirth);
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);

            // assert registration pathway data
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Active).ToList().Count.Should().Be(expectedRegistrationProfile.TqRegistrationPathways.Count);
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Transferred).ToList().Count.Should().Be(1);

            foreach (var expectedPathway in expectedRegistrationProfile.TqRegistrationPathways)
            {
                var actualActivePathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(p => p.TqProviderId == expectedPathway.TqProviderId
                                                                                                                && p.Status == expectedPathway.Status);
                // Assert Active Registration
                AssertRegistrations(actualActivePathway, expectedPathway);

                var actualTransferredPathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(p => p.Status == Common.Enum.RegistrationPathwayStatus.Transferred);
                // Assert Transferred Registration
                AssertRegistrations(actualActivePathway, expectedPathway);

                // Assert Active Pathway Assessment
                var actualActiveAssessment = actualActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
                AssertAssessments(actualActiveAssessment, actualActivePathway);

                // Assert Transferred Pathway Assessment
                var actualTransferredAssessment = actualTransferredPathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
                AssertAssessments(actualTransferredAssessment, actualTransferredPathway);

                // Assert Active PathwayResult
                var actualActiveResults = actualActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
                AssertResults(actualActiveResults, actualActiveAssessment);

                // Assert Transferred PathwayResult
                var actualTransferredResults = actualTransferredAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
                AssertResults(actualTransferredResults, actualTransferredAssessment);
            }
        }

        private static void AssertRegistrations(TqRegistrationPathway actualPathway, TqRegistrationPathway expectedPathway)
        {
            actualPathway.Should().NotBeNull();
            actualPathway.TqProviderId.Should().Be(expectedPathway.TqProviderId);
            actualPathway.AcademicYear.Should().Be(expectedPathway.AcademicYear);
            actualPathway.Status.Should().Be(expectedPathway.Status);
            actualPathway.IsBulkUpload.Should().Be(expectedPathway.IsBulkUpload);

            // assert specialisms
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

        private static void AssertAssessments(TqPathwayAssessment actualAssessment, TqRegistrationPathway tqRegistrationPathway)
        {
            actualAssessment.Should().NotBeNull();
            actualAssessment.TqRegistrationPathwayId.Should().Be(tqRegistrationPathway.Id);
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeTrue();

            if (tqRegistrationPathway.Status == Common.Enum.RegistrationPathwayStatus.Active)
                actualAssessment.EndDate.Should().BeNull();
            else
                actualAssessment.EndDate.Should().NotBeNull();
        }

        private static void AssertResults(TqPathwayResult actualResult, TqPathwayAssessment tqAssessment)
        {
            actualResult.Should().NotBeNull();
            actualResult.TqPathwayAssessmentId.Should().Be(tqAssessment.Id);
            actualResult.IsOptedin.Should().BeTrue();
            actualResult.IsBulkUpload.Should().BeTrue();

            if (tqAssessment.TqRegistrationPathway.Status == Common.Enum.RegistrationPathwayStatus.Active)
                actualResult.EndDate.Should().BeNull();
            else
                actualResult.EndDate.Should().NotBeNull();
        }
    }
}
