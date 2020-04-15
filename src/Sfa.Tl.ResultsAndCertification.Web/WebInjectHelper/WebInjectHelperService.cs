using Sfa.Tl.ResultsAndCertification.Models.Configuration;

namespace Sfa.Tl.ResultsAndCertification.Web.WebInjectHelper
{
    public class WebInjectHelperService : IWebInjectHelperService
    {
        public readonly ResultsAndCertificationConfiguration _configuration;
        public WebInjectHelperService(ResultsAndCertificationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetFeedbackEmailAddress()
        {
            return _configuration.FeedbackEmailAddress;
        }
    }
}
