using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PostResultsServiceTests
{
    public class When_AppealGradeAfterDeadlineRequest_IsCalled : PostResultsServiceServiceBaseTest
    {
        private bool _actualResult;

        public override void Given()
        {
            CreateMapper();
            SeedNotificationTestData();

            Configuration = new ResultsAndCertificationConfiguration
            {
                GovUkNotifyApiKey = "GovUkNotifyApiKey",
                TechnicalSupportEmailAddress = "technical@test.com"
            };

            PostResultsServiceRepository = new PostResultsServiceRepository(DbContext);
            var pathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultsRepository = new GenericRepository<TqPathwayResult>(pathwayResultRepositoryLogger, DbContext);

            var specialismResultRepositoryLogger = new Logger<GenericRepository<TqSpecialismResult>>(new NullLoggerFactory());
            SpecialismResultsRepository = new GenericRepository<TqSpecialismResult>(specialismResultRepositoryLogger, DbContext);

            NotificationsClient = Substitute.For<IAsyncNotificationClient>();
            NotificationLogger = new Logger<NotificationService>(new NullLoggerFactory());
            NotificationTemplateRepositoryLogger = new Logger<GenericRepository<NotificationTemplate>>(new NullLoggerFactory());
            NotificationTemplateRepository = new GenericRepository<NotificationTemplate>(NotificationTemplateRepositoryLogger, DbContext);
            NotificationService = new NotificationService(NotificationTemplateRepository, NotificationsClient, NotificationLogger);

            PostResultsServiceServiceLogger = new Logger<PostResultsServiceService>(new NullLoggerFactory());
            PostResultsServiceService = new PostResultsServiceService(Configuration, PostResultsServiceRepository, PathwayResultsRepository, SpecialismResultsRepository, NotificationService, PostResultsServiceMapper, PostResultsServiceServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(AppealGradeAfterDeadlineRequest request)
        {
            _actualResult = await PostResultsServiceService.AppealGradeAfterDeadlineRequestAsync(request);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(AppealGradeAfterDeadlineRequest request, bool result)
        {
            await WhenAsync(request);
            _actualResult.Should().Be(result);

            if (result)
            {
                await NotificationsClient.Received(2).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>());
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { new AppealGradeAfterDeadlineRequest { ProfileId = 1, AssessmentId = 2, ResultId = 3, RequestedUserEmailAddress = "test@test.com" }, true }
                };
            }
        }
    }
}
