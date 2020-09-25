using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    [Collection("BulkRegistration")]
    public class When_CompareAndProcessRegistrationsAsync_Called_With_Valid_Registrations : IClassFixture<BulkRegistrationsTextFixture>
    {
        private RegistrationProcessResponse _result;
        private BulkRegistrationsTextFixture _bulkRegistrationTestFixture;

        public When_CompareAndProcessRegistrationsAsync_Called_With_Valid_Registrations(BulkRegistrationsTextFixture bulkRegistrationTestFixture)
        {
            // Given
            _bulkRegistrationTestFixture = bulkRegistrationTestFixture;
            _bulkRegistrationTestFixture.Uln = 1111111111;
            _bulkRegistrationTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson);            
            _bulkRegistrationTestFixture.TqRegistrationProfilesData = _bulkRegistrationTestFixture.GetRegistrationsDataToProcess(new List<long> { _bulkRegistrationTestFixture.Uln });
        }

        [Fact(Skip ="Waiting for Infrastructure to setup integration tests")]
        public async Task Then_Expected_Registrations_Are_Created()
        {
            // when
            await _bulkRegistrationTestFixture.WhenAsync();

            // then
            _result = _bulkRegistrationTestFixture.Result;
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeTrue();
            _result.BulkUploadStats.Should().NotBeNull();
            _result.BulkUploadStats.TotalRecordsCount.Should().Be(_bulkRegistrationTestFixture.TqRegistrationProfilesData.Count);
            _result.BulkUploadStats.NewRecordsCount.Should().Be(1);
            _result.BulkUploadStats.AmendedRecordsCount.Should().Be(0);
            _result.BulkUploadStats.UnchangedRecordsCount.Should().Be(0);
            _result.ValidationErrors.Should().BeNullOrEmpty();

            var expectedRegistrationProfile = _bulkRegistrationTestFixture.TqRegistrationProfilesData.FirstOrDefault(p => p.UniqueLearnerNumber == _bulkRegistrationTestFixture.Uln);

            var actualRegistrationProfile = await _bulkRegistrationTestFixture.DbContext.TqRegistrationProfile.Where(x => x.UniqueLearnerNumber == _bulkRegistrationTestFixture.Uln)
                                                                                                              .Include(x => x.TqRegistrationPathways)
                                                                                                                  .ThenInclude(x => x.TqRegistrationSpecialisms)
                                                                                                              .Include(x => x.TqRegistrationPathways).FirstOrDefaultAsync();

            // assert registration profile data
            actualRegistrationProfile.Should().NotBeNull();
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);
            actualRegistrationProfile.Firstname.Should().Be(expectedRegistrationProfile.Firstname);
            actualRegistrationProfile.Lastname.Should().Be(expectedRegistrationProfile.Lastname);
            actualRegistrationProfile.DateofBirth.Should().Be(expectedRegistrationProfile.DateofBirth);
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);

            // assert registration pathway data
            actualRegistrationProfile.TqRegistrationPathways.Count.Should().Be(expectedRegistrationProfile.TqRegistrationPathways.Count);

            foreach(var expectedPathway in actualRegistrationProfile.TqRegistrationPathways)
            {
                var actualPathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(p => p.TqProviderId == expectedPathway.TqProviderId
                                                                                                    && p.Status == expectedPathway.Status);
                actualPathway.Should().NotBeNull();
                actualPathway.TqProviderId.Should().Be(expectedPathway.TqProviderId);
                actualPathway.AcademicYear.Should().Be(expectedPathway.AcademicYear);
                actualPathway.Status.Should().Be(expectedPathway.Status);
                actualPathway.IsBulkUpload.Should().Be(expectedPathway.IsBulkUpload);

                // assert specialisms
                actualPathway.TqRegistrationSpecialisms.Count.Should().Be(expectedPathway.TqRegistrationSpecialisms.Count);

                foreach(var expectedSpecialism in expectedPathway.TqRegistrationSpecialisms)
                {
                    var actualSpecialism = actualPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.TlSpecialismId == expectedSpecialism.TlSpecialismId);

                    actualSpecialism.Should().NotBeNull();
                    actualSpecialism.TlSpecialismId.Should().Be(expectedSpecialism.TlSpecialismId);
                    actualSpecialism.IsOptedin.Should().Be(expectedSpecialism.IsOptedin);
                    actualSpecialism.IsBulkUpload.Should().Be(expectedSpecialism.IsBulkUpload);
                }
            }
        }     
    }
}
