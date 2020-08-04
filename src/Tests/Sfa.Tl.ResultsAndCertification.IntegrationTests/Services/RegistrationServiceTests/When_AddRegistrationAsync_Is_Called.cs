using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_AddRegistrationAsync_Is_Called : RegistrationServiceBaseTest
    {
        private Task<bool> _result;
        private RegistrationRequest _registrationRequest;

        public override void Given()
        {
            CreateMapper();
            SeedTestData(EnumAwardingOrganisation.Pearson);
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, RegistrationMapper, RegistrationRepositoryLogger);
            _registrationRequest = new RegistrationRequest
            {
                AoUkprn = TlAwardingOrganisation.UkPrn,
                FirstName = "First",
                LastName = "Last",
                DateOfBirth = "13/01/1987".ToDateTime(),
                ProviderUkprn = TlProvider.UkPrn,
                RegistrationDate = DateTime.UtcNow,
                CoreCode = Pathway.LarId,
                SpecialismCodes = Specialisms.Select(s => s.LarId),
                CreatedBy = "Test User"
            };
        }

        public override void When()
        {
            _result = RegistrationService.AddRegistrationAsync(_registrationRequest);
        }

        [Fact]
        public void Then_Expected_Response_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Result.Should().BeTrue();            
        }
    }
}
