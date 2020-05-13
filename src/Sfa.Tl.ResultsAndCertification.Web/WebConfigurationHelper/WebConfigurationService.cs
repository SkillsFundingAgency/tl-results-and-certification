using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using HeaderContent = Sfa.Tl.ResultsAndCertification.Web.Content.Layout.Header;

namespace Sfa.Tl.ResultsAndCertification.Web.WebConfigurationHelper
{
    public class WebConfigurationService : IWebConfigurationService
    {
        public readonly ResultsAndCertificationConfiguration _configuration;
        public WebConfigurationService(ResultsAndCertificationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetFeedbackEmailAddress()
        {
            return _configuration.FeedbackEmailAddress;
        }

        public string GetSignOutPath()
        {
            return _configuration.DfeSignInSettings.SignOutEnabled ? RouteConstants.SignOutDsi : RouteConstants.SignOut;
        }

        public string FormatPageTitle(string pageTitle)
        {
            var titleSuffix = $"{HeaderContent.Service_Name_Title} – {HeaderContent.Logo_Text}";

            if (string.IsNullOrWhiteSpace(pageTitle))
                return titleSuffix;

            return $"{pageTitle} – {titleSuffix}";
        }
    }
}
