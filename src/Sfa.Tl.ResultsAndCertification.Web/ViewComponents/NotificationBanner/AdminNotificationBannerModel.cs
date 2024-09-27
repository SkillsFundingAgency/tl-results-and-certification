namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner
{
    public class AdminNotificationBannerModel : NotificationBannerModel
    {
        public AdminNotificationBannerModel(string message)
        {
            IsRawHtml = true;
            DisplayMessageBody = true;

            Message = string.IsNullOrWhiteSpace(message) 
                ? string.Empty : 
                $"<b>{message}</b>";
        }
    }
}