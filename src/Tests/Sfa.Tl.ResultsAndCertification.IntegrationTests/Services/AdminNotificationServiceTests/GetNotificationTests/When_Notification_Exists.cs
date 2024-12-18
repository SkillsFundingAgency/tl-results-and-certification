using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminNotificationServiceTests.GetNotificationTests
{
    public class When_Notification_Exist : GetNotificationBaseTest
    {
        private const int ExistingNotificationId = 1;

        public override void Given()
        {
            NotificationId = ExistingNotificationId;
            SeedTestData();
        }

        [Fact]
        public void Then_Should_Return_Expected()
        {
            Result.Should().NotBeNull();

            Result.Id.Should().Be(ExistingNotificationId);
            Result.Title.Should().Be(FirstNotification.Title);
            Result.Content.Should().Be(FirstNotification.Content);
            Result.Target.Should().Be(FirstNotification.Target);
            Result.Start.Should().Be(FirstNotification.Start);
            Result.End.Should().Be(FirstNotification.End);
        }
    }
}