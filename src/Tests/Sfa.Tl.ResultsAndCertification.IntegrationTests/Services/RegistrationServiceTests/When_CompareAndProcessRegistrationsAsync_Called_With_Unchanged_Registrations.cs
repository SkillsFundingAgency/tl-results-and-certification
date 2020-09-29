using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    [Collection("BulkRegistration")]
    public class When_CompareAndProcessRegistrationsAsync_Called_With_Unchanged_Registrations : IClassFixture<BulkRegistrationsTextFixture>
    {
        private RegistrationProcessResponse _result;
        private BulkRegistrationsTextFixture _bulkRegistrationTestFixture;

        public When_CompareAndProcessRegistrationsAsync_Called_With_Unchanged_Registrations(BulkRegistrationsTextFixture bulkRegistrationTestFixture)
        {
            _bulkRegistrationTestFixture = bulkRegistrationTestFixture;
            
            // Given
            _bulkRegistrationTestFixture.Uln = 1111111111;
            _bulkRegistrationTestFixture.SeedTestData(EnumAwardingOrganisation.Pearson);

            var barnsleyCollegeTqProvider = _bulkRegistrationTestFixture.TqProviders.FirstOrDefault(p => p.TlProvider.UkPrn == 10000536);
            _bulkRegistrationTestFixture.SeedRegistrationData(_bulkRegistrationTestFixture.Uln, barnsleyCollegeTqProvider);

            _bulkRegistrationTestFixture.TqRegistrationProfilesData = _bulkRegistrationTestFixture.GetRegistrationsDataToProcess(new List<long> { _bulkRegistrationTestFixture.Uln }, barnsleyCollegeTqProvider);
        }

        [Fact(Skip = "Waiting for Infrastructure to setup integration tests")]
        public async Task Then_Expected_Registrations_Are_Unchanged()
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
            _result.BulkUploadStats.AmendedRecordsCount.Should().Be(0);
            _result.BulkUploadStats.UnchangedRecordsCount.Should().Be(1);
            _result.ValidationErrors.Should().BeNullOrEmpty();
        }
    }
}
