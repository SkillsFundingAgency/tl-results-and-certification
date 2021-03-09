using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TrainingProviderServiceTests
{
    public class When_FindLearnerRecord_IsCalled : TrainingProviderServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedRegistrationsData(_ulns, TqProvider);
            SeedRegistrationTransfer(1111111113, Provider.BarsleyCollege, Provider.WalsallCollege);

            // Create Service
            CreateMapper();
            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);
            TrainingProviderService = new TrainingProviderService(RegistrationPathwayRepository, TrainingProviderMapper);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(long uln, Provider provider, FindLearnerRecord expectedResult)
        {
            var actualResult = TrainingProviderService.FindLearnerRecordAsync((long)provider, uln).Result;

            if (expectedResult == null)
            {
                actualResult.Should().BeNull();
                return;
            }

            actualResult.Should().NotBeNull();
            actualResult.Uln.Should().Be(expectedResult.Uln);
            actualResult.IsLearnerRegistered.Should().Be(expectedResult.IsLearnerRegistered);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, null }, // Invalid Uln

                    new object[] { 1111111111, Provider.BarsleyCollege, new FindLearnerRecord { Uln = 1111111111, IsLearnerRegistered = true } },
                    new object[] { 1111111111, Provider.WalsallCollege, null }, // Uln not from WalsallCollege

                    new object[] { 1111111112, Provider.BarsleyCollege, new FindLearnerRecord { Uln = 1111111112, IsLearnerRegistered = true } }, // Withdrawn
                    new object[] { 1111111113, Provider.BarsleyCollege, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = false } }, // Transferred
                    new object[] { 1111111113, Provider.WalsallCollege, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = true } }
                };
            }
        }

        private void SeedRegistrationTransfer(long uln, Provider transferFrom, Provider transferTo)
        {
            var toProvider = DbContext.TlProvider.FirstOrDefault(x => x.UkPrn == (long)transferTo);
            var transferToTqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, toProvider, true);
            var transferFromTqProvider = DbContext.TqProvider.FirstOrDefault(x => x.TlProvider.UkPrn == (long)transferFrom);

            SeedRegistrationData(uln, RegistrationPathwayStatus.Transferred, transferFromTqProvider);
            SeedRegistrationData(uln, RegistrationPathwayStatus.Active, transferToTqProvider);
        }
    }

    public enum Provider
    {
        BarsleyCollege = 10000536,
        WalsallCollege = 10007315
    }
}
