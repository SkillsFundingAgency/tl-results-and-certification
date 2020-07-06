using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_CompareAndProcessRegistrationsAsync_Is_Called_With_Valid_Registrations : IClassFixture<BulkRegistrationsTextFixture>
    {
        private RegistrationProcessResponse _result;
        private BulkRegistrationsTextFixture _bulkRegistrationTestFixture;

        public When_CompareAndProcessRegistrationsAsync_Is_Called_With_Valid_Registrations(BulkRegistrationsTextFixture bulkRegistrationTestFixture)
        {
            _bulkRegistrationTestFixture = bulkRegistrationTestFixture;
            _bulkRegistrationTestFixture.DbCheckpoint.TablesToInclude = new string[] { "TqRegistrationProfile", "TqRegistrationPathway", "TqRegistrationSpecialism" };
            _bulkRegistrationTestFixture.Uln = 1111111111;
            _bulkRegistrationTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson, true);            
            _bulkRegistrationTestFixture.TqRegistrationProfilesData = _bulkRegistrationTestFixture.GetRegistrationsDataToProcess(new List<long> { _bulkRegistrationTestFixture.Uln });
        }

        [Fact(Skip = "Still in progress")]
        public async Task Then_Expected_ValidationResults_Are_Returned()
        {
            await _bulkRegistrationTestFixture.WhenAsync();
            _result = _bulkRegistrationTestFixture.Result;
            _result.Should().NotBeNull();
            _result.IsSuccess.Should().BeTrue();
            _result.ValidationErrors.Should().BeNullOrEmpty();

            var expectedRegistrationProfile = _bulkRegistrationTestFixture.TqRegistrationProfilesData.FirstOrDefault(p => p.UniqueLearnerNumber == _bulkRegistrationTestFixture.Uln);
            var actualRegistrationProfile = _bulkRegistrationTestFixture.RegistrationRepository.GetFirstOrDefaultAsync(p => p.UniqueLearnerNumber == _bulkRegistrationTestFixture.Uln).Result;

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
                actualPathway.RegistrationDate.Should().Be(expectedPathway.RegistrationDate);
                actualPathway.Status.Should().Be(expectedPathway.Status);
                actualPathway.IsBulkUpload.Should().Be(expectedPathway.IsBulkUpload);

                // assert specialisms
                actualPathway.TqRegistrationSpecialisms.Count.Should().Be(expectedPathway.TqRegistrationSpecialisms.Count);

                foreach(var expectedSpecialism in expectedPathway.TqRegistrationSpecialisms)
                {
                    var actualSpecialism = actualPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.TlSpecialismId == expectedSpecialism.TlSpecialismId);

                    actualSpecialism.Should().NotBeNull();
                    actualSpecialism.TlSpecialismId.Should().Be(expectedSpecialism.TlSpecialismId);
                    actualSpecialism.Status.Should().Be(expectedSpecialism.Status);
                    actualSpecialism.IsBulkUpload.Should().Be(expectedSpecialism.IsBulkUpload);
                }
            }
        }
    }
}
