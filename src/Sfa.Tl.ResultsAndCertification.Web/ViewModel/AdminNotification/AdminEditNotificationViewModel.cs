using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification
{
    public class AdminEditNotificationViewModel : AdminNotificationBaseViewModel
    {
        public int NotificationId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminNotificationDetails,
            RouteAttributes = new Dictionary<string, string>
            {
                ["notificationId"] = NotificationId.ToString()
            }
        };
    }
}