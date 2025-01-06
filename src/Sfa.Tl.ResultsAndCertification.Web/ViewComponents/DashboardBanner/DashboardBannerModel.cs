namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.DashboardBanner
{
    public class DashboardBannerModel
    {
        public string Message { get; set; } = string.Empty;

        public DashboardBannerModel(string message)
        {
            Message = message ?? string.Empty;
        }
    }
}