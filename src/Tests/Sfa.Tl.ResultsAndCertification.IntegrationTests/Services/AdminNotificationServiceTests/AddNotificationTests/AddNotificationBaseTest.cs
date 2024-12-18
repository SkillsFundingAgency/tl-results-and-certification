using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminNotificationServiceTests.AddNotificationTests
{
    public abstract class AddNotificationBaseTest : AdminNotificationServiceBaseTest
    {
        protected AddNotificationRequest Request { get; set; }

        protected AddNotificationResponse Result { get; set; }

        public override async Task When()
        {
            Result = await AdminNotificationService.AddNotificationAsync(Request);
        }
    }
}