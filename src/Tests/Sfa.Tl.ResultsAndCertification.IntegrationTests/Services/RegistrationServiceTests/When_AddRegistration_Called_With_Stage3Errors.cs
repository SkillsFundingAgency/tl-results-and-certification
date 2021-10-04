using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_AddRegistration_Called_With_Stage3Errors : RegistrationServiceBaseTest
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
                CoreCode = "123", // Invalid core
                SpecialismCodes = Specialisms.Select(s => s.LarId),
                PerformedBy = "Test User"
            };
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await RegistrationService.AddRegistrationAsync(_registrationRequest);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long providerUkprn, string coreCode, List<string> specialismCodes)
        {
            _registrationRequest.ProviderUkprn = providerUkprn;
            _registrationRequest.CoreCode = coreCode;
            _registrationRequest.SpecialismCodes = specialismCodes;

            await WhenAsync();

            _result.Should().BeFalse();
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                var tlProvider = new TlProviderBuilder().Build();
                var tlPathway = new TlPathwayBuilder().Build(EnumAwardingOrganisation.Pearson);
                var tlSpecialisms = new TlSpecialismBuilder().BuildList(EnumAwardingOrganisation.Pearson, tlPathway);
                return new[]
                {
                    new object[] { 10000000, tlPathway.LarId, new List<string> { tlSpecialisms.Last().LarId } }, // ProviderNotRegisteredWithAo
                    new object[] { tlProvider.UkPrn, "00000000", new List<string> { tlSpecialisms.Last().LarId } }, // CoreNotRegisteredWithProvider
                    new object[] { tlProvider.UkPrn, tlPathway.LarId, new List<string> { "XYZ456125" } }, // SpecialismNotValidWithCore
                    new object[] { tlProvider.UkPrn, tlPathway.LarId, new List<string> { tlSpecialisms.First().LarId } }, // SpecialismCannotBeSelectedAsSingleOption
                    new object[] { tlProvider.UkPrn, tlPathway.LarId, new List<string> { { tlSpecialisms.First().LarId }, { tlSpecialisms.Last().LarId } } }, // SpecialismIsNotValid
                };
            }
        }
    }
}

