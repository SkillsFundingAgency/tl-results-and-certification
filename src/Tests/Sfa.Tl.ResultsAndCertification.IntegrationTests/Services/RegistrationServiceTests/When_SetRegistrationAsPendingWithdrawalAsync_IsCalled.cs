using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_SetRegistrationAsPendingWithdrawalAsync_IsCalled : RegistrationServiceBaseTest
    {
        private static long _providerUkprn = 10000536;

        public override void Given()
        {
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            long uln = 1111111111;
            SeedRegistrationDataActivePendingWithdrawalFalse(uln);
            SeedRegistrationDataActivePendingWithdrawalTrue(uln);
            SeedRegistrationDataWithdrawn(uln);

            CreateMapper();

            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqPathwayAssessmentRepository = new GenericRepository<TqPathwayAssessment>(TqPathwayAssessmentRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqPathwayAssessmentRepository, TqRegistrationSpecialismRepository, CommonService, SystemProvider, RegistrationMapper, RegistrationRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(SetRegistrationAsPendingWithdrawalRequest request, bool expectedResponse)
        {
            bool actualResult = await RegistrationService.SetRegistrationAsPendingWithdrawalAsync(request);
            actualResult.Should().Be(expectedResponse);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new SetRegistrationAsPendingWithdrawalRequest
                        {
                            ProviderUkprn = _providerUkprn,
                            ProfileId = 1, //Profile => Active, pending withdrawal false
                            PerformedBy = "Test User"
                        },
                        true
                    },
                    new object[]
                    {
                        new SetRegistrationAsPendingWithdrawalRequest
                        {
                            ProviderUkprn = _providerUkprn,
                            ProfileId = 2, //Profile => Active, pending withdrawal true
                            PerformedBy = "Test User"
                        },
                        false
                    },
                    new object[]
                    {
                        new SetRegistrationAsPendingWithdrawalRequest
                        {
                            ProviderUkprn = _providerUkprn,
                            ProfileId = 3, //Profile => Withdrawn
                            PerformedBy = "Test User"
                        },
                        false
                    },
                    new object[]
                    {
                        new SetRegistrationAsPendingWithdrawalRequest
                        {
                            ProviderUkprn = _providerUkprn,
                            ProfileId = 99, //Non-existing profile
                            PerformedBy = "Test User"
                        },
                        false
                    }
                };
            }
        }

        protected override void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway);
            var tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, TlProvider);
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);

            DbContext.SaveChangesAsync();
        }

        private void SeedRegistrationDataActivePendingWithdrawalFalse(long uln)
        {
            SeedRegistrationData(uln, r => { });
        }

        private void SeedRegistrationDataActivePendingWithdrawalTrue(long uln)
        {
            SeedRegistrationData(uln, r => r.IsPendingWithdrawal = true);
        }

        private void SeedRegistrationDataWithdrawn(long uln)
        {
            SeedRegistrationData(uln, r => r.Status = Common.Enum.RegistrationPathwayStatus.Withdrawn);
        }

        private void SeedRegistrationData(long uln, Action<TqRegistrationPathway> registrationAction)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);

            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProvider);
            registrationAction(tqRegistrationPathway);

            DbContext.SaveChangesAsync();
        }
    }
}
