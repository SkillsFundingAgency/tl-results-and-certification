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
    public class When_AddRegistrationAsync_Called_With_Valid_Data : RegistrationServiceBaseTest
    {
        private bool _result;
        private RegistrationRequest _registrationRequest;
        
        public override void Given()
        {
            CreateMapper();
            SeedTestData(EnumAwardingOrganisation.Pearson);
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqRegistrationSpecialismRepository, CommonService, RegistrationMapper, RegistrationRepositoryLogger);

            _registrationRequest = new RegistrationRequest
            {
                AoUkprn = TlAwardingOrganisation.UkPrn,
                FirstName = "First",
                LastName = "Last",
                DateOfBirth = "11/01/1987".ToDateTime(),
                ProviderUkprn = TlProvider.UkPrn,
                AcademicYear = DateTime.UtcNow.Year,
                CoreCode = Pathway.LarId,
                SpecialismCodes = Specialisms.Select(s => s.LarId),
                PerformedBy = "Test User"
            };
        }

        public async override Task When()
        {
            _result = await RegistrationService.AddRegistrationAsync(_registrationRequest);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().BeTrue();            
        }
    }
}
