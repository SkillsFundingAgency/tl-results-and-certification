using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests.GetRegisteredProviderCoreDetailsAsync
{
    public class When_GetRegisteredProviderCoreDetailsAsync_IsCalled_With_Valid_Ukprn : RegistrationServiceBaseTest
    {
        private IList<CoreDetails> _result;

        public override void Given()
        {
            SeedTestData();
            CreateMapper();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, RegistrationMapper);
        }

        public override void When()
        {
            _result = RegistrationService.GetRegisteredProviderCoreDetails(TlAwardingOrganisation.UkPrn, TlProvider.UkPrn).Result;
        }

        [Fact]
        public void Then_Expected_Results_Is_Returned()
        {
            _result.Should().NotBeNull();

            var actualResult = _result.FirstOrDefault();
            actualResult.Id.Should().Be(Pathway.Id);
            actualResult.CoreCode.Should().Be(Pathway.LarId);
            actualResult.CoreName.Should().Be(Pathway.Name);
        }

        [Fact]
        public void Then_Core_Details_Count_Is_As_Expected()
        {
            _result.Count().Should().Be(1);
        }
    }
}
