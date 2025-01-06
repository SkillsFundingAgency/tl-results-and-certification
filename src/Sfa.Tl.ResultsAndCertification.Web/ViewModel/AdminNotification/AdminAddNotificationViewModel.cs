using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification
{
    public class AdminAddNotificationViewModel : AdminNotificationBaseViewModel
    {
        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminFindNotification
        };
    }
}