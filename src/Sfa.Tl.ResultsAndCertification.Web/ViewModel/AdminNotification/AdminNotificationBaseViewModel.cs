using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification
{
    public abstract class AdminNotificationBaseViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public NotificationTarget Target { get; set; }

        public string StartDate => $"{StartYear}/{StartMonth}/{StartDay}";

        public string StartDay { get; set; }

        public string StartMonth { get; set; }

        public string StartYear { get; set; }

        public string EndDate => $"{EndYear}/{EndMonth}/{EndDay}";

        public string EndDay { get; set; }

        public string EndMonth { get; set; }

        public string EndYear { get; set; }
    }
}