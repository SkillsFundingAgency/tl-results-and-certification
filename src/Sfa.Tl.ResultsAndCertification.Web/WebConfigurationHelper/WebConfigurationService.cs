using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using HeaderContent = Sfa.Tl.ResultsAndCertification.Web.Content.Layout.Header;

namespace Sfa.Tl.ResultsAndCertification.Web.WebConfigurationHelper
{
    public class WebConfigurationService : IWebConfigurationService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        public WebConfigurationService(ResultsAndCertificationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetFeedbackEmailAddress()
        {
            return _configuration.FeedbackEmailAddress;
        }

        public string GetTechnicalSupportEmailAddress()
        {
            return _configuration.TechnicalSupportEmailAddress;
        }

        public string GetSignOutPath()
        {
            return _configuration.DfeSignInSettings.SignOutEnabled ? RouteConstants.SignOutDsi : RouteConstants.SignOut;
        }

        public string GetFormattedTitle(string title, bool isModelValid)
        {
            var titleSuffix = $"{HeaderContent.Service_Name_Title} – {HeaderContent.Logo_Text}";
            var formattedTitle = string.IsNullOrWhiteSpace(title) ? titleSuffix : $"{title} – {titleSuffix}";
            return isModelValid ? formattedTitle : $"{HeaderContent.Error_Text} {formattedTitle}";
        }

        public int GetSessionTimeoutValue()
        {
            return _configuration.DfeSignInSettings.Timeout;
        }
    }
}
