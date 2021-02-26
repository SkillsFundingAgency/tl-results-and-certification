using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    [Collection("BulkProcessTests")]
    public class When_ReJoinRegistration_IsCalled_With_Results_Data : IClassFixture<BulkRegistrationsTextFixture>
    {
        private bool _result;
        private BulkRegistrationsTextFixture _bulkRegistrationTestFixture;

        public When_ReJoinRegistration_IsCalled_With_Results_Data(BulkRegistrationsTextFixture bulkRegistrationTestFixture)
        {
            _bulkRegistrationTestFixture = bulkRegistrationTestFixture;

            // Given
            _bulkRegistrationTestFixture.Uln = 1111111111;
            _bulkRegistrationTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson);            

            var registrationRecord = _bulkRegistrationTestFixture.SeedRegistrationData(_bulkRegistrationTestFixture.Uln, null, RegistrationPathwayStatus.Withdrawn, isBulkUpload: false);

            _bulkRegistrationTestFixture.TqRegistrationProfilesData = new List<TqRegistrationProfile> { registrationRecord };

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var seededRegistrationPathways = registrationRecord.TqRegistrationPathways.ToList();
            tqPathwayAssessmentsSeedData.AddRange(_bulkRegistrationTestFixture.GetPathwayAssessmentsDataToProcess(seededRegistrationPathways, seedPathwayAssessmentsAsActive: false, isBulkUpload: false));
            var _pathwayAssessments = _bulkRegistrationTestFixture.SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            // Results seed
            _bulkRegistrationTestFixture.SeedPathwayResultsData(_bulkRegistrationTestFixture.GetPathwayResultsDataToProcess(_pathwayAssessments, seedPathwayResultsAsActive: false, isHistorical: true, isBulkUpload: false));
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            // when
            var profileId = _bulkRegistrationTestFixture.DbContext.TqRegistrationProfile.AsNoTracking().FirstOrDefault(x => x.UniqueLearnerNumber == _bulkRegistrationTestFixture.Uln)?.Id;
            var reJoinRegistrationRequest = new RejoinRegistrationRequest
            {
                ProfileId = profileId ?? 0,
                AoUkprn = _bulkRegistrationTestFixture.TlAwardingOrganisation.UkPrn,
                PerformedBy = "Test User"
            };
            await _bulkRegistrationTestFixture.WhenReJoinAsync(reJoinRegistrationRequest);

            // then
            _result = _bulkRegistrationTestFixture.RejoinResult;
            _result.Should().BeTrue();

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

            // Assert registration pathway data
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == RegistrationPathwayStatus.Active).ToList().Count.Should().Be(1);
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == RegistrationPathwayStatus.Withdrawn).ToList().Count.Should().Be(1);

            // Assert Any Active Pathway
            actualRegistrationProfile.TqRegistrationPathways.Any(x => x.Status == RegistrationPathwayStatus.Active).Should().BeTrue();

            // Assert Active Pathway
            var actualActivePathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => x.EndDate == null && x.Status == RegistrationPathwayStatus.Active);
            var expectedActivePathway = expectedRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
            AssertRegistrationPathway(actualActivePathway, expectedActivePathway, false);

            // Assert Withdrawn PathwayAssessment
            var actualActiveAssessment = actualActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousAssessment = expectedActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayAssessment(actualActiveAssessment, expectedPreviousAssessment, isRejoin: true);

            // Assert Active PathwayResult
            var actualActiveResult = actualActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousResult = expectedPreviousAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayResults(actualActiveResult, expectedPreviousResult, isRejoin: true);
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

        public static void AssertPathwayAssessment(TqPathwayAssessment actualAssessment, TqPathwayAssessment expectedAssessment, bool isRejoin = false)
        {
            actualAssessment.Should().NotBeNull();
            if (!isRejoin)
                actualAssessment.TqRegistrationPathwayId.Should().Be(expectedAssessment.TqRegistrationPathwayId);

            actualAssessment.TqRegistrationPathway.TqProviderId.Should().Be(expectedAssessment.TqRegistrationPathway.TqProviderId);
            actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId);
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeFalse();

            if (actualAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualAssessment.EndDate.Should().BeNull();
            else
                actualAssessment.EndDate.Should().NotBeNull();
        }

        public static void AssertPathwayResults(TqPathwayResult actualResult, TqPathwayResult expectedResult, bool isRejoin = false)
        {
            actualResult.Should().NotBeNull();
            if (!isRejoin)
                actualResult.TqPathwayAssessmentId.Should().Be(expectedResult.TqPathwayAssessmentId);

            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualResult.IsOptedin.Should().BeTrue();
            actualResult.IsBulkUpload.Should().BeFalse();

            if (actualResult.TqPathwayAssessment.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualResult.EndDate.Should().BeNull();
            else
                actualResult.EndDate.Should().NotBeNull();
        }
    }
}
