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
                ResultAwarded = "Distinction*", CalculationStatus = CalculationStatus.Completed, CertificateType = PrintCertificateType.Certificate, StartDate = DateTime.UtcNow.AddMonths(-1), IsOptedin = true, EndDate = DateTime.UtcNow, CreatedOn = DateTime.UtcNow } }, true);


            // Seed print certificate
            PrintBatchItem printBatchItem = _bulkRegistrationTestFixture.SeedPrintBatchItem();

            var printCertificate = new PrintCertificate
            {
                Uln = registrationRecord.UniqueLearnerNumber,
                LearnerName = $"{registrationRecord.Firstname} {registrationRecord.Lastname}",
                Type = PrintCertificateType.Certificate,
                LearningDetails = "test-learning-details",
                DisplaySnapshot = "test-display-snapshot",
                IsReprint = false,
                LastRequestedOn = new DateTime(2023, 1, 1),
                TqRegistrationPathway = seededRegistrationPathways.First(),
                PrintBatchItem = printBatchItem
            };

            PrintCertificateDataProvider.CreatePrintCertificate(_bulkRegistrationTestFixture.DbContext, printCertificate);
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
                                                                                                                        .Include(x => x.TqRegistrationPathways)
                                                                                                                            .ThenInclude(x => x.PrintCertificates)
                                                                                                                            .ThenInclude(x => x.PrintBatchItem)
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
            AssertRegistrationPathway(actualActivePathway, expectedActivePathway);

            // Assert Withdrawn PathwayAssessment
            var actualActiveAssessment = actualActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousAssessment = expectedActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayAssessment(actualActiveAssessment, expectedPreviousAssessment);

            // Assert Active PathwayResult
            var actualActiveResult = actualActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousResult = expectedPreviousAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayResults(actualActiveResult, expectedPreviousResult);

            // Assert Withdrawn SpecialismAssessment
            var actualActiveSpecialism = actualActivePathway.TqRegistrationSpecialisms.FirstOrDefault(x => x.EndDate == null);
            var expectedActiveSpecialism = expectedActivePathway.TqRegistrationSpecialisms.FirstOrDefault(x => x.EndDate != null);

            var actualActiveSpecialismAssessment = actualActiveSpecialism.TqSpecialismAssessments.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousSpecialismAssessment = expectedActiveSpecialism.TqSpecialismAssessments.FirstOrDefault(x => x.EndDate != null);
            AssertSpecialismAssessment(actualActiveSpecialismAssessment, expectedPreviousSpecialismAssessment);

            // Assert Active SpecialismResult
            var actualActiveSpecialismResult = actualActiveSpecialismAssessment.TqSpecialismResults.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousSpecialismResult = expectedPreviousSpecialismAssessment.TqSpecialismResults.FirstOrDefault(x => x.EndDate != null);
            AssertSpecialismResult(actualActiveSpecialismResult, expectedPreviousSpecialismResult);

            // Assert IndustryPlacement Data
            var actualActiveIndustryPlacement = actualActivePathway.IndustryPlacements.FirstOrDefault();
            var expectedPreviousIndustryPlacement = expectedActivePathway.IndustryPlacements.FirstOrDefault();

            actualActiveIndustryPlacement.Status.Should().Be(expectedPreviousIndustryPlacement.Status);
            actualActiveIndustryPlacement.Details.Should().Be(expectedPreviousIndustryPlacement.Details);

            // Assert Active Overall result
            var actualActiveOverallResult = actualActivePathway.OverallResults.FirstOrDefault(ovr => ovr.EndDate == null);
            var expectedActiveOverallResult = expectedActivePathway.OverallResults.FirstOrDefault(ovr => ovr.EndDate != null);
            AssertOverallResult(actualActiveOverallResult, expectedActiveOverallResult);

            // Assert Active Overall result
            var actualPrintCertificate = actualActivePathway.PrintCertificates.FirstOrDefault();
            var expectedPrintCertificate = expectedActivePathway.PrintCertificates.FirstOrDefault();
            AssertPrintCertificate(actualPrintCertificate, expectedPrintCertificate);
        }

        public static void AssertRegistrationPathway(TqRegistrationPathway actualPathway, TqRegistrationPathway expectedPathway)
        {
            actualPathway.Should().NotBeNull();

            actualPathway.Id.Should().BeGreaterThan(expectedPathway.Id);
            actualPathway.TqProviderId.Should().Be(expectedPathway.TqProviderId);
            actualPathway.AcademicYear.Should().Be(expectedPathway.AcademicYear);
            actualPathway.Status.Should().Be(RegistrationPathwayStatus.Active);

            actualPathway.IsBulkUpload.Should().Be(expectedPathway.IsBulkUpload);

            // Assert specialisms
            actualPathway.TqRegistrationSpecialisms.Should().HaveCount(expectedPathway.TqRegistrationSpecialisms.Count);

            foreach (var expectedSpecialism in expectedPathway.TqRegistrationSpecialisms)
            {
                var actualSpecialism = actualPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.TlSpecialismId == expectedSpecialism.TlSpecialismId);

                actualSpecialism.Should().NotBeNull();
                actualSpecialism.TlSpecialismId.Should().Be(expectedSpecialism.TlSpecialismId);
                actualSpecialism.IsOptedin.Should().Be(expectedSpecialism.IsOptedin);
                actualSpecialism.IsBulkUpload.Should().Be(expectedSpecialism.IsBulkUpload);
            }
        }

        public static void AssertPathwayAssessment(TqPathwayAssessment actualAssessment, TqPathwayAssessment expectedAssessment)
        {
            actualAssessment.Should().NotBeNull();
            actualAssessment.TqRegistrationPathwayId.Should().BeGreaterThan(expectedAssessment.TqRegistrationPathwayId);
            actualAssessment.TqRegistrationPathway.TqProviderId.Should().Be(expectedAssessment.TqRegistrationPathway.TqProviderId);
            actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId);
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeFalse();
            actualAssessment.EndDate.Should().BeNull();
        }

        public static void AssertPathwayResults(TqPathwayResult actualResult, TqPathwayResult expectedResult)
        {
            actualResult.Should().NotBeNull();
            actualResult.TqPathwayAssessmentId.Should().BeGreaterThan(expectedResult.TqPathwayAssessmentId);
            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualResult.IsOptedin.Should().BeTrue();
            actualResult.IsBulkUpload.Should().BeFalse();
            actualResult.EndDate.Should().BeNull();
        }

        public static void AssertSpecialismAssessment(TqSpecialismAssessment actualAssessment, TqSpecialismAssessment expectedAssessment)
        {
            actualAssessment.Should().NotBeNull();
            actualAssessment.TqRegistrationSpecialismId.Should().BeGreaterThan(expectedAssessment.TqRegistrationSpecialismId);
            actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId);
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeFalse();
            actualAssessment.EndDate.Should().BeNull();
        }

        public static void AssertSpecialismResult(TqSpecialismResult actualResult, TqSpecialismResult expectedResult)
        {
            actualResult.Should().NotBeNull();
            actualResult.TqSpecialismAssessmentId.Should().BeGreaterThan(expectedResult.TqSpecialismAssessmentId);
            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualResult.IsOptedin.Should().BeTrue();
            actualResult.IsBulkUpload.Should().BeFalse();
            actualResult.EndDate.Should().BeNull();
        }

        private void AssertOverallResult(OverallResult actualOverallResult, OverallResult expectedOverallResult)
        {
            actualOverallResult.Should().NotBeNull();

            actualOverallResult.TqRegistrationPathwayId.Should().BeGreaterThan(expectedOverallResult.TqRegistrationPathwayId);
            actualOverallResult.TqRegistrationPathway.TqProviderId.Should().Be(expectedOverallResult.TqRegistrationPathway.TqProviderId);
            actualOverallResult.Details.Should().Be(expectedOverallResult.Details);
            actualOverallResult.ResultAwarded.Should().Be(expectedOverallResult.ResultAwarded);
            actualOverallResult.CalculationStatus.Should().Be(expectedOverallResult.CalculationStatus);
            actualOverallResult.CertificateType.Should().Be(expectedOverallResult.CertificateType);
            actualOverallResult.PrintAvailableFrom.Should().Be(expectedOverallResult.PrintAvailableFrom);
            actualOverallResult.PublishDate.Should().Be(expectedOverallResult.PublishDate);
            actualOverallResult.EndDate.Should().BeNull();
            actualOverallResult.IsOptedin.Should().BeTrue();
        }

        private void AssertPrintCertificate(PrintCertificate actualPrintCertificate, PrintCertificate expectedPrintCertificate)
        {
            actualPrintCertificate.Should().NotBeNull();

            actualPrintCertificate.TqRegistrationPathwayId.Should().BeGreaterThan(expectedPrintCertificate.TqRegistrationPathwayId);
            actualPrintCertificate.Uln.Should().Be(expectedPrintCertificate.Uln);
            actualPrintCertificate.LearnerName.Should().Be(expectedPrintCertificate.LearnerName);
            actualPrintCertificate.Type.Should().Be(expectedPrintCertificate.Type);
            actualPrintCertificate.LearningDetails.Should().Be(expectedPrintCertificate.LearningDetails);
            actualPrintCertificate.DisplaySnapshot.Should().Be(expectedPrintCertificate.DisplaySnapshot);
            actualPrintCertificate.IsReprint.Should().Be(expectedPrintCertificate.IsReprint);
            actualPrintCertificate.LastRequestedOn.Should().BeNull();

            PrintBatchItem actualPrintBatchItem = actualPrintCertificate.PrintBatchItem;
            PrintBatchItem expectedPrintBatchItem = expectedPrintCertificate.PrintBatchItem;

            actualPrintBatchItem.Should().NotBeNull();
            actualPrintBatchItem.BatchId.Should().Be(expectedPrintBatchItem.BatchId);
            actualPrintBatchItem.TlProviderAddressId.Should().Be(expectedPrintBatchItem.TlProviderAddressId);
            actualPrintBatchItem.Status.Should().Be(expectedPrintBatchItem.Status);
            actualPrintBatchItem.Reason.Should().Be(expectedPrintBatchItem.Reason);
            actualPrintBatchItem.TrackingId.Should().Be(expectedPrintBatchItem.TrackingId);
            actualPrintBatchItem.SignedForBy.Should().Be(expectedPrintBatchItem.SignedForBy);
            actualPrintBatchItem.StatusChangedOn.Should().Be(expectedPrintBatchItem.StatusChangedOn);
        }
    }
}