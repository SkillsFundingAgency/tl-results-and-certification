using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_TransformRegistrationModel_IsCalled : RegistrationServiceBaseTest
    {
        private IList<TqRegistrationProfile> _result;
        private IList<RegistrationRecordResponse> _stage4RegistrationsData;
        private string _performedBy = "System";

        public override void Given()
        {
            CreateMapper();
            SeedTestData(EnumAwardingOrganisation.Pearson);
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, RegistrationMapper, RegistrationRepositoryLogger);
            _stage4RegistrationsData = new RegistrationsStage4Builder().BuildValidList();
        }

        public override Task When()
        {            
            var transformModelTask = Task.Run(() => RegistrationService.TransformRegistrationModel(_stage4RegistrationsData, _performedBy));
            _result = transformModelTask.GetAwaiter().GetResult();
            return transformModelTask;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.Count.Should().Be(_stage4RegistrationsData.Count);

            foreach (var expectedRegistration in _stage4RegistrationsData)
            {
                var actualRegistration = _result.FirstOrDefault(x => x.UniqueLearnerNumber == expectedRegistration.Uln);

                actualRegistration.Should().NotBeNull();
                actualRegistration.UniqueLearnerNumber.Should().Be(expectedRegistration.Uln);
                actualRegistration.Firstname.Should().Be(expectedRegistration.FirstName);
                actualRegistration.Lastname.Should().Be(expectedRegistration.LastName);
                actualRegistration.DateofBirth.Should().Be(expectedRegistration.DateOfBirth);

                var actualPathway = actualRegistration.TqRegistrationPathways.FirstOrDefault(p => p.TqProviderId == expectedRegistration.TqProviderId);
                actualPathway.Should().NotBeNull();
                actualPathway.TqProviderId.Should().Be(expectedRegistration.TqProviderId);
                actualPathway.TqProvider.TlProviderId.Should().Be(expectedRegistration.TlProviderId);
                actualPathway.TqProvider.TqAwardingOrganisationId.Should().Be(expectedRegistration.TqAwardingOrganisationId);
                actualPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisatonId.Should().Be(expectedRegistration.TlAwardingOrganisatonId);
                actualPathway.TqProvider.TqAwardingOrganisation.TlPathwayId.Should().Be(expectedRegistration.TlPathwayId);

                var expectedSpecialismId = expectedRegistration.TlSpecialismLarIds.First().Key;
                var actualSpecialism = actualPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.TlSpecialismId == expectedSpecialismId);
                actualSpecialism.Should().NotBeNull();
            }
        }
    }
}
