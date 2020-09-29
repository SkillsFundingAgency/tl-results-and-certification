using FluentAssertions;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.NotificationServiceTests
{
    public class When_Called_To_Send_Email_Failed : NotificationServiceBaseTest
    {
        private string _errorMessage;
        private string _templateName;
        public override void Given()
        {
            SeedNotificationTestData();
            _templateName = "WrongTemplate";
            _errorMessage = $"Notification email template {_templateName} not found";
            NotificationsClient = Substitute.For<IAsyncNotificationClient>();
            NotificationLogger = Substitute.For<ILogger<NotificationService>>();
            NotificationService = new NotificationService(Repository, NotificationsClient, NotificationLogger);
        }

        public async override Task When()
        {
            ToAddress = "test@test.com";
            var tokens = new Dictionary<string, dynamic>
            {
                { "tlevel_name",  "Digital" }
            };
            Result = await NotificationService.SendEmailNotificationAsync(_templateName, ToAddress, tokens);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeFalse();
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            NotificationsClient.DidNotReceive().SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>());
        }

        [Fact]
        public void Then_Returns_Expected_Errors()
        {
            NotificationLogger.Received(1).LogWarning(Arg.Any<EventId>(), _errorMessage);
        }
    }
}
