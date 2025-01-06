using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminNotificationServiceTests.GetNotificationTests
{
    public class When_Notification_Doesnt_Exist : GetNotificationBaseTest
    {
        private const int NonExistingNotificationId = 999;

        public override void Given()
        {
            NotificationId = NonExistingNotificationId;
            SeedTestData();
        }

        [Fact]
        public void Then_Should_Return_Null()
        {
            Result.Should().BeNull();
        }
    }
}