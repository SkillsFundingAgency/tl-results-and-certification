using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminNotificationServiceTests.GetNotificationTests
{
    public abstract class GetNotificationBaseTest : AdminNotificationServiceBaseTest
    {
        protected int NotificationId { get; set; }

        protected GetNotificationResponse Result { get; set; }

        public override async Task When()
        {
            Result = await AdminNotificationService.GetNotificationAsync(NotificationId);
        }
    }
}