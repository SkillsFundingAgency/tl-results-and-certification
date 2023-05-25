namespace Sfa.Tl.ResultsAndCertification.Web.ViewComponents.InformationBanner
{
    public class InformationBannerModel
    {
        public string Heading { get; }

        public string Message { get; }

        public InformationBannerModel(string message)
            : this(Content.ViewComponents.InformationBanner.Heading, message)
        {
        }

        public InformationBannerModel(string heading, string message)
        {
            Heading = heading;
            Message = message;
        }
    }
}