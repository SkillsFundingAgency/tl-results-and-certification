namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner
{
    public class NotificationBannerModel
    {
        public string HeaderMessage { get; set; }
        public string Message { get; set; }
        public bool IsPrsJourney { get; set; } // TODO: Ravi rename this to ShowMessageBody
    }
}