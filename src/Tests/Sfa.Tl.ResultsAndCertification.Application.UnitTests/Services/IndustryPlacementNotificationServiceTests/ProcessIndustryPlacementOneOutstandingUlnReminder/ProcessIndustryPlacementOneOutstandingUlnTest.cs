using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.IndustryPlacementNotificationServiceTests.ProcessIndustryPlacementMissedDeadlineReminder
{
    public class ProcessIndustryPlacementOneOutstandingUlnReminderTest : IndustryPlacementNotificationServiceBaseTest
    {
        protected IndustryPlacementNotificationResponse Result;
        public override void Given()
        {
            CreateData();

            ILogger<GenericRepository<TqRegistrationPathway>> RegistrationPathwaytRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwaytRepositoryLogger, DbContext);

            CommonRepository.GetCurrentAcademicYearsAsync().Returns(CreateAcademicYears());

            NotificationService.SendEmailNotificationAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>())
                .Returns(true);

            DfeSignInApiClient.GetDfeUsersAllProviders(Arg.Any<List<long>>())
                .Returns(CreateDfeUsers());

            IndustryPlacementNotificationService = new(
                    RegistrationPathwayRepository,
                    NotificationTemplateRepository,
                    ProviderRepository,
                    CommonRepository,
                    DfeSignInApiClient,
                    NotificationService,
                    NotificationClient,
                    Logger);
        }

        public async override Task When()
        {
            Result = await IndustryPlacementNotificationService.ProcessIndustryPlacementOneOutstandingUlnReminderAsync();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonRepository.Received(1).GetCurrentAcademicYearsAsync();
            NotificationService.Received(1).SendEmailNotificationAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>());
            DfeSignInApiClient.Received(1).GetDfeUsersAllProviders(Arg.Any<List<long>>());
        }

        [Fact]
        public void Then_Returns_Expected_Data()
        {
            Result.UsersCount.Equals(1);
            Result.EmailSentCount.Equals(1);
            Result.IsSuccess.Equals(true);
        }
    }
}
