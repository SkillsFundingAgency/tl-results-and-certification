using FluentAssertions;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.NotificationServiceTests
{
    public class When_SendEmailNotificationAsync_IsCalled_To_Send_Email_Returns_True : NotificationServiceBaseTest
    {
        public override void Given()
        {
            SeedNotificationTestData();
            
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
            Result = NotificationService.SendEmailNotificationAsync(NotificationTemplate.TemplateName, ToAddress, tokens).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_SendEmailNotificationAsync_Returns_True()
        {
            Result.Should().BeTrue();
        }

        [Fact]
        public void Then_NotificationsClient_SendEmail_Is_Called_Once_With_Expected_Values()
        {
            NotificationsClient
                .Received(1)
                .SendEmailAsync(Arg.Is<string>(emailAddress => emailAddress == ToAddress), 
                                Arg.Is<string>(templateId => templateId == NotificationTemplate.TemplateId.ToString()),
                                Arg.Is<Dictionary<string, dynamic>>(dict => dict.First().Key == "tlevel_name"));
        }
    }
}
