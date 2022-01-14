using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
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

            _bulkRegistrationTestFixture.TqRegistrationProfileBeforeSeed = _bulkRegistrationTestFixture.SeedRegistrationData(_bulkRegistrationTestFixture.Uln, barnsleyCollegeTqProvider, seedIndustryPlacement: true);

            // PathwayAssessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var seededRegistrationPathways = _bulkRegistrationTestFixture.TqRegistrationProfileBeforeSeed.TqRegistrationPathways.ToList();
            tqPathwayAssessmentsSeedData.AddRange(_bulkRegistrationTestFixture.GetPathwayAssessmentsDataToProcess(seededRegistrationPathways));
            var _pathwayAssessments = _bulkRegistrationTestFixture.SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            // PathwayResults seed
            var results = _bulkRegistrationTestFixture.GetPathwayResultsDataToProcess(_pathwayAssessments);
            var _pathwayResults = _bulkRegistrationTestFixture.SeedPathwayResultsData(results);

            // SpecialismAssessments seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var seededRegistrationSpecialisms = _bulkRegistrationTestFixture.TqRegistrationProfileBeforeSeed.TqRegistrationPathways.SelectMany(x => x.TqRegistrationSpecialisms);
            tqSpecialismAssessmentsSeedData.AddRange(_bulkRegistrationTestFixture.GetSpecialismAssessmentsDataToProcess(seededRegistrationSpecialisms));
            _bulkRegistrationTestFixture.SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData);

            // Input param
            var registrationDataToProcess = _bulkRegistrationTestFixture.GetRegistrationsDataToProcess(_bulkRegistrationTestFixture.Uln, walsallCollegeTqProvider);
            registrationDataToProcess.Id = 0 - Constants.RegistrationProfileStartIndex;

            var pathwayIndex = 0;
            foreach (var pathway in registrationDataToProcess.TqRegistrationPathways)
            {
                pathway.Id = pathwayIndex - Constants.RegistrationPathwayStartIndex;
            }

            var specialismIndex = 0;
            foreach (var sp in registrationDataToProcess.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms))
            {
                sp.Id = specialismIndex - Constants.RegistrationSpecialismsStartIndex;
                specialismIndex++;
            }
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
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                            .ThenInclude(x => x.IndustryPlacements)
                                                                                                                       .FirstOrDefault();
            // Assert registration profile data
            actualRegistrationProfile.Should().NotBeNull();
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);
            actualRegistrationProfile.Firstname.Should().Be(expectedRegistrationProfile.Firstname);
            actualRegistrationProfile.Lastname.Should().Be(expectedRegistrationProfile.Lastname);
            actualRegistrationProfile.DateofBirth.Should().Be(expectedRegistrationProfile.DateofBirth);
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);

            // Assert registration pathway data
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Active).ToList().Count.Should().Be(1);
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Transferred).ToList().Count.Should().Be(1);

            // Assert Transferred Pathway
            var actualTransferredPathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => _bulkRegistrationTestFixture.TqRegistrationProfileBeforeSeed.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
            var expectedTransferredPathway = _bulkRegistrationTestFixture.TqRegistrationProfileBeforeSeed.TqRegistrationPathways.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
            AssertRegistrationPathway(actualTransferredPathway, expectedTransferredPathway);

            // Assert Active Pathway
            var activePathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => _bulkRegistrationTestFixture.TqRegistrationProfilesData.FirstOrDefault().TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
            var expectedActivePathway = _bulkRegistrationTestFixture.TqRegistrationProfilesData.FirstOrDefault().TqRegistrationPathways.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
            AssertRegistrationPathway(activePathway, expectedActivePathway);

            // Assert Active PathwayAssessment
            var actualActiveAssessment = activePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
            var expectedActiveAssessment = expectedActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
            AssertPathwayAssessment(actualActiveAssessment, expectedActiveAssessment);

            // Assert Transferred PathwayAssessment
            var actualTransferredAssessment = actualTransferredPathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
            var expectedTransferredAssessment = expectedTransferredPathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayAssessment(actualTransferredAssessment, expectedTransferredAssessment);

            // Assert Active PathwayResult
            var actualActiveResult = actualActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
            var expectedActiveResult = expectedActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
            AssertPathwayResults(actualActiveResult, expectedActiveResult);

            // Assert Transferred PathwayResult
            var actualTransferredResult = actualTransferredAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
            var expectedTransferredResult = expectedTransferredAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayResults(actualTransferredResult, expectedTransferredResult);

            // Assert Active SpecialismAssessment
            var actualActiveSpecialismAssessment = activePathway.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null).SelectMany(s => s.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.EndDate == null));
            var expectedActiveSpecialismAssessment = expectedActivePathway.TqRegistrationSpecialisms.Where(s => s.IsOptedin && s.EndDate == null).SelectMany(s => s.TqSpecialismAssessments.Where(sa => sa.IsOptedin && sa.EndDate == null));
            AssertSpecialismAssessments(actualActiveSpecialismAssessment, expectedActiveSpecialismAssessment);

            // Assert Transferred SpecialismAssessment
            var actualTransferredSpecialismAssessment = actualTransferredPathway.TqRegistrationSpecialisms.Where(s => !s.IsOptedin && s.EndDate != null).SelectMany(s => s.TqSpecialismAssessments.Where(sa => !sa.IsOptedin && sa.EndDate != null));
            var expectedTransferredSpecialismAssessment = expectedTransferredPathway.TqRegistrationSpecialisms.Where(s => !s.IsOptedin && s.EndDate != null).SelectMany(s => s.TqSpecialismAssessments.Where(sa => !sa.IsOptedin && sa.EndDate != null));
            AssertSpecialismAssessments(actualTransferredSpecialismAssessment, expectedTransferredSpecialismAssessment);

            // Assert IndustryPlacement Data
            var actualActiveIndustryPlacement = activePathway.IndustryPlacements.FirstOrDefault();
            var expectedPreviousIndustryPlacement = expectedTransferredPathway.IndustryPlacements.FirstOrDefault();

            actualActiveIndustryPlacement.Status.Should().Be(expectedPreviousIndustryPlacement.Status);
        }

        private static void AssertRegistrationPathway(TqRegistrationPathway actualPathway, TqRegistrationPathway expectedPathway)
        {
            actualPathway.Should().NotBeNull();
            actualPathway.TqProviderId.Should().Be(expectedPathway.TqProviderId);
            actualPathway.AcademicYear.Should().Be(expectedPathway.AcademicYear);
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

        private static void AssertPathwayAssessment(TqPathwayAssessment actualAssessment, TqPathwayAssessment expectedAssessment)
        {
            actualAssessment.Should().NotBeNull();
            actualAssessment.TqRegistrationPathwayId.Should().Be(expectedAssessment.TqRegistrationPathwayId);
            actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId); 
            actualAssessment.IsOptedin.Should().BeTrue();
            actualAssessment.IsBulkUpload.Should().BeTrue();

            if (actualAssessment.TqRegistrationPathway.Status == Common.Enum.RegistrationPathwayStatus.Active)
                actualAssessment.EndDate.Should().BeNull();
            else
                actualAssessment.EndDate.Should().NotBeNull();
        }

        private static void AssertPathwayResults(TqPathwayResult actualResult, TqPathwayResult expectedResult)
        {
            actualResult.Should().NotBeNull();
            actualResult.TqPathwayAssessmentId.Should().Be(expectedResult.TqPathwayAssessmentId);
            actualResult.TlLookupId.Should().Be(expectedResult.TlLookupId);
            actualResult.IsOptedin.Should().BeTrue();
            actualResult.IsBulkUpload.Should().BeTrue();

            if (actualResult.TqPathwayAssessment.TqRegistrationPathway.Status == Common.Enum.RegistrationPathwayStatus.Active)
                actualResult.EndDate.Should().BeNull();
            else
                actualResult.EndDate.Should().NotBeNull();
        }

        private void AssertSpecialismAssessments(IEnumerable<TqSpecialismAssessment> actualAssessments, IEnumerable<TqSpecialismAssessment> expectedAssessments)
        {
            actualAssessments.Should().NotBeEmpty();
            actualAssessments.Should().HaveSameCount(expectedAssessments);
            
            foreach (var expectedAssessment in expectedAssessments)
            {
                var actualAssessment = actualAssessments.FirstOrDefault(x => x.TqRegistrationSpecialismId == expectedAssessment.TqRegistrationSpecialismId);
                actualAssessment.Should().NotBeNull(); 
                actualAssessment.TqRegistrationSpecialismId.Should().Be(expectedAssessment.TqRegistrationSpecialismId);
                actualAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.AssessmentSeriesId);
                actualAssessment.IsOptedin.Should().BeTrue();
                actualAssessment.IsBulkUpload.Should().BeTrue();

                if (actualAssessment.TqRegistrationSpecialism.TqRegistrationPathway.Status == Common.Enum.RegistrationPathwayStatus.Active)
                    actualAssessment.EndDate.Should().BeNull();
                else
                    actualAssessment.EndDate.Should().NotBeNull();

            }
        }
    }
}
