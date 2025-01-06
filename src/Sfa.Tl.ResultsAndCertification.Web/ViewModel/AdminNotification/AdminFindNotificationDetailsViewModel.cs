using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification
{
    public class AdminFindNotificationDetailsViewModel
    {
        public string EndDate { get; set; }

        public string Target { get; set; }

        public string IsActive { get; set; }

        public ChangeRecordModel NotificationDetailsLink { get; set; }
    }
}