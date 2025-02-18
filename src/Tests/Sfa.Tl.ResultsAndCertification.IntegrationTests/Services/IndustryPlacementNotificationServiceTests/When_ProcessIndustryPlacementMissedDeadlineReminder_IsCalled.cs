using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementNotificationServiceTests
{
    public class When_ProcessIndustryPlacementMissedDeadlineReminder_IsCalled : IndustryPlacementNotificationServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<TqRegistrationProfile> _profiles;
        IndustryPlacementNotificationResponse _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
             {
                 { 1111111111, RegistrationPathwayStatus.Active },
                 { 1111111112, RegistrationPathwayStatus.Withdrawn },
                 { 1111111113, RegistrationPathwayStatus.Active },
                 { 1111111114, RegistrationPathwayStatus.Active },
                 { 1111111115, RegistrationPathwayStatus.Active },
                 { 1111111116, RegistrationPathwayStatus.Active },
             };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Ncfe, true);
            _profiles = SeedRegistrationsData(_ulns, TqProvider);

            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);

            CommonRepository = new CommonRepository(DbContext);

            IndustryPlacementNotificationServiceLogger = new Logger<IndustryPlacementNotificationService>(new NullLoggerFactory());

            DfeSignInApiClient.GetDfeUsersAllProviders(Arg.Any<List<long>>()).Returns(new List<DfeUsers> {
                {
                    new DfeUsers { Ukprn = TqProvider.TlProvider.UkPrn.ToString(),
                     Users = new List<ServiceUser> {
                        new ServiceUser { Email = "test@test.com",
                            FirstName = "Test",
                            LastName = "Test",
                            UserStatus = DfeUserStatus.Active,
                            Roles = new List<string> { "Test" }}}}
                }
            });

            NotificationService.SendEmailNotificationAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>()).Returns(true);

            IndustryPlacementNotificationService = new IndustryPlacementNotificationService(RegistrationPathwayRepository,
                NotificationTemplateRepository, TlProviderRepository, CommonRepository, DfeSignInApiClient, NotificationService, NotificationClient,
                IndustryPlacementNotificationServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _actualResult = await IndustryPlacementNotificationService.ProcessIndustryPlacementMissedDeadlineReminderAsync();
        }

        [Fact]
        public async Task Then_Expected_Methods_Are_Called()
        {
            await WhenAsync();
            await NotificationService.Received(1).SendEmailNotificationAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>());
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            _actualResult.Should().NotBeNull();
            _actualResult.IsSuccess.Should().BeTrue();
            _actualResult.UsersCount.Should().Be(1);
            _actualResult.EmailSentCount.Should().Be(1);
            _actualResult.Message.Should().Be($"Total users: {_actualResult.UsersCount} Email sent: {_actualResult.EmailSentCount}.");
        }
    }
}
