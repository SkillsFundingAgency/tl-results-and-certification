using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PostResultsServiceTests
{
    public class When_PrsGradeChangeRequest_IsCalled : PostResultsServiceServiceBaseTest
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

        public async Task WhenAsync(PrsGradeChangeRequest request)
        {
            _actualResult = await PostResultsServiceService.PrsGradeChangeRequestAsync(request);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(PrsGradeChangeRequest request, string technicalTeamEmailTemplateId)
        {
            await WhenAsync(request);

            _actualResult.Should().BeTrue();

            await NotificationsClient.Received(1).SendEmailAsync("technical@test.com", technicalTeamEmailTemplateId, Arg.Any<Dictionary<string, dynamic>>());
            await NotificationsClient.Received(1).SendEmailAsync("test@test.com", "91b21a18-8555-45b8-9739-f18a90228211", Arg.Any<Dictionary<string, dynamic>>());
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new PrsGradeChangeRequest
                        {
                            ComponentType = ComponentType.Core,
                            LearnerName = "John Smith",
                            Uln = 1234567890,
                            ProviderUkprn = 10000536,
                            ComponentName = "Science",
                            ExamPeriod = "Summer 2024",
                            Grade = "B",
                            RequestedMessage = "Test",
                            RequestedUserEmailAddress = "test@test.com"
                        },
                        "daa342c6-8f92-4695-80ee-f251a7844449"
                    },
                    new object[]
                    {
                        new PrsGradeChangeRequest
                        {
                            ComponentType = ComponentType.Specialism,
                            LearnerName = "John Smith",
                            Uln = 1234567890,
                            ProviderUkprn = 10000536,
                            ComponentName = "Food Sciences",
                            ExamPeriod = "Summer 2024",
                            Grade = "A",
                            RequestedMessage = "Test",
                            RequestedUserEmailAddress = "test@test.com"
                        },
                        "559a3508-432d-4dd2-ac87-e7e15c81bcf3"
                    }
                };
            }
        }
    }
}
