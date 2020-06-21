using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations.ValidationErrorsBuilder;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_ValidateRegistrationTlevelsAsync_IsCalled_With_InValid_Registrations : RegistrationServiceBaseTest
    {
        private readonly long _aoUkprn = 10011881;
        private IList<RegistrationRecordResponse> _result;      
        private IList<RegistrationCsvRecordResponse> _stage3RegistrationsData;
        private IList<RegistrationValidationError> _expectedValidationErrors;

        public override void Given()
        {
            SeedTestData();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository);
            _stage3RegistrationsData = new RegistrationsStage3Builder().BuildInvalidList();
            _expectedValidationErrors = new BulkRegistrationValidationErrorsBuilder().BuildStage3ValidationErrorsList();
        }

        public override void When()
        {
            _result = RegistrationService.ValidateRegistrationTlevelsAsync(_aoUkprn, _stage3RegistrationsData).Result;
        }

        [Fact]
        public void Then_Expected_ValidationResults_Are_Returned()
        {
            _result.Should().NotBeNull();
            _result.Count.Should().Be(_stage3RegistrationsData.Count);

            var actualValidationErrors = _result.SelectMany(x => x.ValidationErrors).ToList();

            actualValidationErrors.Count.Should().Be(_expectedValidationErrors.Count);
            actualValidationErrors.Should().BeEquivalentTo(_expectedValidationErrors);
        }
    }
}
