using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using System;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminNotificationServiceTests.UpdateNotificationTests
{
    public class When_IsCalled_Returns_Expected : UpdateNotificationBaseTest
    {
        private const int NotificationId = 1;

        public override void Given()
        {
            Request = new UpdateNotificationRequest
            {
                NotificationId = NotificationId,
                Title = "updated-notification-title",
                Content = "updated-notification-content",
                Target = NotificationTarget.Provider,
                Start = new DateTime(2025, 1, 15),
                End = new DateTime(2025, 1, 27),
                ModifiedBy = "test-user"
            };

            SeedTestData();
        }

        [Fact]
        public void Then_Should_Return_True()
        {
            Result.Should().BeTrue();

            Notification notificationDb = DbContext.Notification.Single(b => b.Id == Request.NotificationId);
            notificationDb.Id.Should().Be(NotificationId);
            notificationDb.Title.Should().Be(Request.Title);
            notificationDb.Content.Should().Be(Request.Content);
            notificationDb.Target.Should().Be(Request.Target);
            notificationDb.Start.Should().Be(Request.Start);
            notificationDb.End.Should().Be(Request.End);
            notificationDb.CreatedBy.Should().Be(FirstNotification.CreatedBy);
            notificationDb.CreatedOn.Should().Be(FirstNotification.CreatedOn);
            notificationDb.ModifiedBy.Should().Be(Request.ModifiedBy);
            notificationDb.ModifiedOn.Should().Be(Now);
        }
    }
}