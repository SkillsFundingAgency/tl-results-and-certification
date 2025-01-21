using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Service;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_ProcessReviewChangeAcademicYear_IsCalled : RegistrationServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;

        public override void Given()
        {
            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, TqProvider);


            CreateMapper();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqPathwayAssessmentRepository = new GenericRepository<TqPathwayAssessment>(TqPathwayAssessmentRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            SystemProvider = new SystemProvider();
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqPathwayAssessmentRepository, TqRegistrationSpecialismRepository, CommonService, SystemProvider, RegistrationMapper, RegistrationRepositoryLogger);
        }

        private bool _actualResult;

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(ChangeAcademicYearRequest request)
        {
            _actualResult = await RegistrationService.ProcessChangeAcademicYearAsync(request);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            ChangeAcademicYearRequest request = new()
            {
                Uln = 1111111111,
                ProfileId = 1,
                AoUkprn = 10011881,
                PerformedBy = "Test user",
                AcademicYearChangeTo = "2022"
            };

            await WhenAsync(request);

            TqRegistrationPathway expected = _registrations.Single(x => x.UniqueLearnerNumber == 1111111111).TqRegistrationPathways.Where(rp => rp.Status == RegistrationPathwayStatus.Active).FirstOrDefault();
            TqRegistrationPathway actual = DbContext.TqRegistrationPathway.FirstOrDefault(ip => ip.Id == request.ProfileId);

            // Assert
            _actualResult.Should().BeTrue();
            expected.AcademicYear.Should().Be(expected.AcademicYear);
        }
    }
}