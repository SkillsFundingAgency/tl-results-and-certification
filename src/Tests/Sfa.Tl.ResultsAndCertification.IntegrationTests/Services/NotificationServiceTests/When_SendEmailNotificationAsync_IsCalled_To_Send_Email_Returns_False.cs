using FluentAssertions;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.NotificationServiceTests
{
    public class When_SendEmailNotificationAsync_IsCalled_To_Send_Email_Returns_False : NotificationServiceBaseTest
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

        public override void When()
        {
            ToAddress = "test@test.com";
            var tokens = new Dictionary<string, dynamic>
            {
                { "tlevel_name",  "Digital" }
            };
            Result = NotificationService.SendEmailNotificationAsync(_templateName, ToAddress, tokens).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_SendEmailNotificationAsync_Returns_False()
        {
            Result.Should().BeFalse();
        }

        [Fact]
        public void Then_NotificationsClient_SendEmailAsync_Is_Not_Called()
        {
            NotificationsClient.DidNotReceive().SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>());
        }

        [Fact]
        public void Then_SendEmailNotificationAsync_Has_Expected_ErrorMessage()
        {
            NotificationLogger.Received(1).LogWarning(Arg.Any<EventId>(), _errorMessage);
        }
    }
}
