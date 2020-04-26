using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;

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
            return _configuration.DfeSignInSettings.EnableSignOut ? RouteConstants.SignOutDsi : RouteConstants.SignOut;
        }
    }
}
