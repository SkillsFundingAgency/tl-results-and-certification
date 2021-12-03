using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_ReJoinRegistration_IsCalled_With_IsRegisteredWithOtherAo : RegistrationServiceBaseTest
    {
        private bool _actualResult;
        private RejoinRegistrationRequest _reJoinRegistrationRequest;

        public override void Given()
        {
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            var pearsonRegistrationProfile = SeedRegistrationDataByStatus(1111111111, RegistrationPathwayStatus.Withdrawn);
            SeedRegistrationDataByStatus(1111111112);

            // Uln 1111111111 is withdrawn from Pearson above registerign with NCFE below. 
            SeedTestData(EnumAwardingOrganisation.Ncfe, true);
            var ncfeRegistrationProfile = SeedRegistrationDataByStatus(1111111111, RegistrationPathwayStatus.Active, null, false);
            ncfeRegistrationProfile.TqRegistrationPathways.ToList().ForEach(x => { x.TqRegistrationProfileId = pearsonRegistrationProfile.Id; });
            pearsonRegistrationProfile.TqRegistrationPathways.Add(ncfeRegistrationProfile.TqRegistrationPathways.ToList()[0]);

            DbContext.SaveChanges();

            CreateMapper();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqRegistrationSpecialismRepository, CommonService, RegistrationMapper, RegistrationRepositoryLogger);

            _reJoinRegistrationRequest = new RejoinRegistrationRequest
            {
                AoUkprn = 10011881,
                PerformedBy = "Test User",
                ProfileId = 1
            };
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _actualResult = await RegistrationService.RejoinRegistrationAsync(_reJoinRegistrationRequest);
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();
            _actualResult.Should().BeFalse();
        }
    }
}
