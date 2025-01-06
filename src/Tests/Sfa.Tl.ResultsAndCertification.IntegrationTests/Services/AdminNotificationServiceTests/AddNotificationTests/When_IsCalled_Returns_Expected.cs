using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using System;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminNotificationServiceTests.AddNotificationTests
{
    public class When_IsCalled_Returns_Expected : AddNotificationBaseTest
    {
        public override void Given()
        {
            Request = new AddNotificationRequest
            {
                Title = "new-banner-title",
                Content = "new-banner-content",
                Target = NotificationTarget.Provider,
                Start = new DateTime(2024, 1, 1),
                End = new DateTime(2024, 1, 1),
                CreatedBy = "test-user"
            };

            SeedTestData();
        }

        [Fact]
        public void Then_Should_Return_True()
        {
            int notificationId = NotificationsDb.Max(b => b.Id) + 1;

            Result.Success.Should().BeTrue();
            Result.NotificationId.Should().Be(notificationId);

            Notification notification = DbContext.Notification.Single(b => b.Title == Request.Title);
            notification.Id.Should().Be(notificationId);
            notification.Title.Should().Be(Request.Title);
            notification.Content.Should().Be(Request.Content);
            notification.Target.Should().Be(Request.Target);
            notification.Start.Should().Be(Request.Start);
            notification.End.Should().Be(Request.End);
            notification.CreatedBy.Should().Be(Request.CreatedBy);
            notification.ModifiedBy.Should().BeNull();
            notification.ModifiedOn.Should().BeNull();
        }
    }
}