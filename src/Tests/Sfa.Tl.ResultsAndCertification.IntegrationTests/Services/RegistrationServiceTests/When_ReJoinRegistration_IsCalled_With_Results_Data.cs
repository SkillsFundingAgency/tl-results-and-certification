using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
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
        private List<OverallGradeLookup> _overallGradeLookup;

        public When_ReJoinRegistration_IsCalled_With_Results_Data(BulkRegistrationsTextFixture bulkRegistrationTestFixture)
        {
            _bulkRegistrationTestFixture = bulkRegistrationTestFixture;

            // Given
            _bulkRegistrationTestFixture.Uln = 1111111111;
            _bulkRegistrationTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson);            

            var registrationRecord = _bulkRegistrationTestFixture.SeedRegistrationData(_bulkRegistrationTestFixture.Uln, null, RegistrationPathwayStatus.Withdrawn, isBulkUpload: false, seedIndustryPlacement: true);

            _bulkRegistrationTestFixture.TqRegistrationProfilesData = new List<TqRegistrationProfile> { registrationRecord };

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var seededRegistrationPathways = registrationRecord.TqRegistrationPathways.ToList();
            tqPathwayAssessmentsSeedData.AddRange(_bulkRegistrationTestFixture.GetPathwayAssessmentsDataToProcess(seededRegistrationPathways, seedPathwayAssessmentsAsActive: false, isBulkUpload: false));
            var pathwayAssessments = _bulkRegistrationTestFixture.SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            // Results seed
            var pathwayResults = _bulkRegistrationTestFixture.SeedPathwayResultsData(_bulkRegistrationTestFixture.GetPathwayResultsDataToProcess(pathwayAssessments, seedPathwayResultsAsActive: false, isHistorical: false, isBulkUpload: false));

            // Specialism Assessments seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var seededRegistrationSpecialisms = registrationRecord.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList();
            tqSpecialismAssessmentsSeedData.AddRange(_bulkRegistrationTestFixture.GetSpecialismAssessmentsDataToProcess(seededRegistrationSpecialisms, seedSpecialismAssessmentsAsActive: false, isBulkUpload: false));
            var specialismAssessments = _bulkRegistrationTestFixture.SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData);

            // Specialisms Results seed
            var specialismResults = _bulkRegistrationTestFixture.SeedSpecialismResultsData(_bulkRegistrationTestFixture.GetSpecialismResultsDataToProcess(specialismAssessments, seedSpecialismResultsAsActive: false, isHistorical: false, isBulkUpload: false));

            // Seed OverallResultLookup
            _overallGradeLookup = new List<OverallGradeLookup>();
            var pathwayId = seededRegistrationPathways.FirstOrDefault().Id;
            var coreResultId = pathwayResults.FirstOrDefault().TlLookupId;
            var splResultId = specialismResults.FirstOrDefault().TlLookupId;
            _overallGradeLookup.Add(new OverallGradeLookup { TlPathwayId = 1, TlLookupCoreGradeId = coreResultId, TlLookupSpecialismGradeId = splResultId, TlLookupOverallGradeId = 1 });
            OverallGradeLookupProvider.CreateOverallGradeLookupList(_bulkRegistrationTestFixture.DbContext, _overallGradeLookup);

            // Seed Overall results
            OverallResultDataProvider.CreateOverallResult(_bulkRegistrationTestFixture.DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = pathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                ResultAwarded = "Distinction*", CalculationStatus = CalculationStatus.Completed, StartDate = DateTime.UtcNow.AddMonths(-1), IsOptedin = true, EndDate = DateTime.UtcNow, CreatedOn = DateTime.UtcNow } }, true);

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
                                                                                                                                .ThenInclude(x => x.TqSpecialismAssessments)
                                                                                                                                    .ThenInclude(x => x.TqSpecialismResults)
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                            .ThenInclude(x => x.TqPathwayAssessments)
                                                                                                                                .ThenInclude(x => x.TqPathwayResults)
                                                                                                                        .Include(x => x.TqRegistrationPathways)
                                                                                                                            .ThenInclude(x => x.IndustryPlacements)
                                                                                                                        .Include(x => x.TqRegistrationPathways)
                                                                                                                            .ThenInclude(x => x.OverallResults)
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

            // Assert Withdrawn SpecialismAssessment
            var actualActiveSpecialism = actualActivePathway.TqRegistrationSpecialisms.FirstOrDefault(x => x.EndDate == null);
            var expectedActiveSpecialism = expectedActivePathway.TqRegistrationSpecialisms.FirstOrDefault(x => x.EndDate != null);
            
            var actualActiveSpecialismAssessment = actualActiveSpecialism.TqSpecialismAssessments.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousSpecialismAssessment = expectedActiveSpecialism.TqSpecialismAssessments.FirstOrDefault(x => x.EndDate != null);
            AssertSpecialismAssessment(actualActiveSpecialismAssessment, expectedPreviousSpecialismAssessment, isRejoin: true);

            // Assert Active SpecialismResult
            var actualActiveSpecialismResult = actualActiveSpecialismAssessment.TqSpecialismResults.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousSpecialismResult = expectedPreviousSpecialismAssessment.TqSpecialismResults.FirstOrDefault(x => x.EndDate != null);
            AssertSpecialismResult(actualActiveSpecialismResult, expectedPreviousSpecialismResult, isRejoin: true);

            // Assert IndustryPlacement Data
            var actualActiveIndustryPlacement = actualActivePathway.IndustryPlacements.FirstOrDefault();
            var expectedPreviousIndustryPlacement = expectedActivePathway.IndustryPlacements.FirstOrDefault();

            actualActiveIndustryPlacement.Status.Should().Be(expectedPreviousIndustryPlacement.Status);
            actualActiveIndustryPlacement.Details.Should().Be(expectedPreviousIndustryPlacement.Details);

            // Assert Active Overall result
            var actualActiveOverallResult = actualActivePathway.OverallResults.FirstOrDefault(ovr => ovr.EndDate == null);
            var expectedActiveOverallResult = expectedActivePathway.OverallResults.FirstOrDefault(ovr => ovr.EndDate != null);
            AssertOverallResult(actualActiveOverallResult, expectedActiveOverallResult, isRejoin: true);
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

        public static void AssertSpecialismAssessment(TqSpecialismAssessment actualAssessment, TqSpecialismAssessment expectedAssessment, bool isRejoin = false, bool isTransferred = false)
        {
            actualAssessment.Should().NotBeNull();
            if (!isRejoin && !isTransferred)
                actualAssessment.TqRegistrationSpecialismId.Should().Be(expectedAssessment.TqRegistrationSpecialismId);

            actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId);
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeFalse();

            if (actualAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualAssessment.EndDate.Should().BeNull();
            else
                actualAssessment.EndDate.Should().NotBeNull();
        }

        public static void AssertSpecialismResult(TqSpecialismResult actualResult, TqSpecialismResult expectedResult, bool isRejoin = false)
        {
            actualResult.Should().NotBeNull();
            if (!isRejoin)
                actualResult.TqSpecialismAssessmentId.Should().Be(expectedResult.TqSpecialismAssessmentId);

            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualResult.IsOptedin.Should().BeTrue();
            actualResult.IsBulkUpload.Should().BeFalse();

            if (actualResult.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualResult.EndDate.Should().BeNull();
            else
                actualResult.EndDate.Should().NotBeNull();
        }

        private void AssertOverallResult(OverallResult actualOverallResult, OverallResult expectedOverallResult, bool isRejoin = false, bool isTransferred = false)
        {
            actualOverallResult.Should().NotBeNull();
            if (!isRejoin && !isTransferred)
                expectedOverallResult.TqRegistrationPathwayId.Should().Be(expectedOverallResult.TqRegistrationPathwayId);

            actualOverallResult.TqRegistrationPathway.TqProviderId.Should().Be(expectedOverallResult.TqRegistrationPathway.TqProviderId);
            actualOverallResult.Details.Should().Be(expectedOverallResult.Details);
            actualOverallResult.ResultAwarded.Should().Be(expectedOverallResult.ResultAwarded);
            actualOverallResult.CalculationStatus.Should().Be(expectedOverallResult.CalculationStatus);
            actualOverallResult.PrintAvailableFrom.Should().Be(expectedOverallResult.PrintAvailableFrom);
            actualOverallResult.PublishDate.Should().Be(expectedOverallResult.PublishDate);

            if (actualOverallResult.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active)
                actualOverallResult.EndDate.Should().BeNull();
            else
                actualOverallResult.EndDate.Should().NotBeNull();
        }
    }
}
