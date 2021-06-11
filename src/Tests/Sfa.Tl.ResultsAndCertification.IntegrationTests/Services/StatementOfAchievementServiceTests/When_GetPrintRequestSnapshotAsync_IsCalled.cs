using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.StatementOfAchievementServiceTests
{
    public class When_GetPrintRequestSnapshotAsync_IsCalled : StatementOfAchievementServiceBaseTest
    {
        private PrintRequestSnapshot _actualResult;

        // Seed data variables
        private IList<TqRegistrationProfile> _profiles;
        private IList<PrintCertificate> _printCertificates;

        public override void Given()
        {
            _profiles = new List<TqRegistrationProfile>();
            var ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Withdrawn },
                { 1111111112, RegistrationPathwayStatus.Withdrawn }
            };

            // Seed Registrations
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _profiles = SeedRegistrationsData(ulns, TqProvider);

            // Seed PrintCertificate
            _printCertificates = SeedPrintCertificates(_profiles.Select(x => x.TqRegistrationPathways.FirstOrDefault()).ToList());

            // dependencies
            StatementOfAchievementRepositoryLogger = new Logger<StatementOfAchievementRepository>(new NullLoggerFactory());
            StatementOfAchievementRepository = new StatementOfAchievementRepository(DbContext, StatementOfAchievementRepositoryLogger);
            StatementOfAchievementServiceLogger = new Logger<StatementOfAchievementService>(new NullLoggerFactory());
            BatchRepositoryLogger = new Logger<GenericRepository<Batch>>(new NullLoggerFactory());
            BatchRepository = new GenericRepository<Batch>(BatchRepositoryLogger, DbContext);
            
            // test service
            StatementOfAchievementService = new StatementOfAchievementService(StatementOfAchievementRepository, BatchRepository, TrainingProviderMapper, StatementOfAchievementServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn, long uln)
        {
            if (_actualResult != null)
                return;

            var profile = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == uln);
            var profileId = profile?.Id ?? 0;
            int pathwayId = profile?.TqRegistrationPathways?.FirstOrDefault(x => x.TqRegistrationProfileId == profileId)?.TqRegistrationProfileId ?? 0;

            _actualResult = await StatementOfAchievementService.GetPrintRequestSnapshotAsync(providerUkprn, profileId, pathwayId);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, Provider provider, bool isRecordFound)
        {
            await WhenAsync((long)provider, uln);

            if (isRecordFound == false)
            {
                _actualResult.Should().BeNull();
                return;
            }

            var expectedPrintCertificate = _printCertificates.FirstOrDefault(x => x.Uln == uln);
            
            expectedPrintCertificate.Should().NotBeNull();
            _actualResult.RegistrationPathwayStatus.Should().Be(expectedPrintCertificate.TqRegistrationPathway.Status);
            _actualResult.RequestDetails.Should().Be(expectedPrintCertificate.DisplaySnapshot);
            _actualResult.RequestedOn.Should().Be(expectedPrintCertificate.CreatedOn);
            _actualResult.RequestedBy.Should().Be(expectedPrintCertificate.CreatedBy);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, false },
                    new object[] { 1111111111, Provider.BarsleyCollege, true },
                    new object[] { 1111111111, Provider.WalsallCollege, false },
                    new object[] { 1111111112, Provider.BarsleyCollege, true },
                    new object[] { 1111111112, Provider.WalsallCollege, false },
                };
            }
        }        
    }
}
