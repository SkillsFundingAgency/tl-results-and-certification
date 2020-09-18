using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_ValidateRegistrationTlevelsAsync_IsCalled_With_Valid_Registrations : RegistrationServiceBaseTest
    {
        private readonly long _aoUkprn = 10011881;
        private IList<RegistrationRecordResponse> _result;
        private IList<RegistrationCsvRecordResponse> _stage3RegistrationsData;

        public override void Given()
        {
            SeedTestData();
            CreateMapper();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, RegistrationMapper, RegistrationRepositoryLogger, TqRegistrationSpecialismRepository);

            _stage3RegistrationsData = new RegistrationsStage3Builder().BuildValidList();
        }

        public async override Task When()
        {
            _result = await RegistrationService.ValidateRegistrationTlevelsAsync(_aoUkprn, _stage3RegistrationsData);
        }

        [Fact]
        public void Then_No_Validation_Errors_Are_Returned()
        {
            _result.Should().NotBeNull();
            _result.Count.Should().Be(_stage3RegistrationsData.Count);

            var actualValidationErrors = _result.SelectMany(x => x.ValidationErrors).ToList();
            actualValidationErrors.Count.Should().Be(0);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Count.Should().Be(_stage3RegistrationsData.Count);

            foreach (var expectedRegistration in _stage3RegistrationsData)
            {
                var actualRegistration = _result.FirstOrDefault(x => x.Uln == expectedRegistration.Uln);
                
                actualRegistration.Should().NotBeNull();
                actualRegistration.Uln.Should().Be(expectedRegistration.Uln);
                actualRegistration.FirstName.Should().Be(expectedRegistration.FirstName);
                actualRegistration.LastName.Should().Be(expectedRegistration.LastName);
                actualRegistration.DateOfBirth.Should().Be(expectedRegistration.DateOfBirth);
                actualRegistration.AcademicYear.Should().Be(expectedRegistration.AcademicYear);
                actualRegistration.TqProviderId.Should().Be(TqProvider.Id);
                actualRegistration.TqAwardingOrganisationId.Should().Be(TqProvider.TqAwardingOrganisationId);
                actualRegistration.TlAwardingOrganisatonId.Should().Be(TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.Id);
                actualRegistration.TlProviderId.Should().Be(TqProvider.TlProviderId);
                actualRegistration.TlSpecialismLarIds.Should().BeEquivalentTo(TqProvider.TqAwardingOrganisation.TlPathway.TlSpecialisms.Select(s => new KeyValuePair<int, string>(s.Id, s.LarId)).Where(s => expectedRegistration.SpecialismCodes.Contains(s.Value)));
            }
        }
    }
}
