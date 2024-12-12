using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminNotificationServiceTests.UpdateNotificationTests
{
    public abstract class UpdateNotificationBaseTest : AdminNotificationServiceBaseTest
    {
        protected UpdateNotificationRequest Request { get; set; }

        protected bool Result { get; set; }

        protected static DateTime Now => new(2024, 1, 1);

        public override async Task When()
        {
            Result = await AdminNotificationService.UpdateNotificationAsync(Request, () => Now);
        }
    }
}