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
using System.Threading.Tasks;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

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
            CreateMapper();
            SeedTestData();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, RegistrationMapper, RegistrationRepositoryLogger, TqRegistrationSpecialismRepository);

            _stage3RegistrationsData = new RegistrationsStage3Builder().BuildInvalidList();
            _expectedValidationErrors = new BulkRegistrationValidationErrorsBuilder().BuildStage3ValidationErrorsList();
        }

        public async override Task When()
        {
            _result = await RegistrationService.ValidateRegistrationTlevelsAsync(_aoUkprn, _stage3RegistrationsData);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Count.Should().Be(_stage3RegistrationsData.Count);

            var actualValidationErrors = _result.SelectMany(x => x.ValidationErrors).ToList();
            actualValidationErrors.Count.Should().Be(_expectedValidationErrors.Count);
            actualValidationErrors.Should().BeEquivalentTo(_expectedValidationErrors);
        }
    }
}
