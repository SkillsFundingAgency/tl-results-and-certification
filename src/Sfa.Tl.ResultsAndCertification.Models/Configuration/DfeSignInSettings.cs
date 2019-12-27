
namespace Sfa.Tl.ResultsAndCertification.Models.Configuration
{
    public class DfeSignInSettings
    {
        public string MetadataAddress { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }
        public string SignedOutCallbackPath { get; set; }
        public string LogoutPath { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string APISecret { get; set; }
        public string APIUri { get; set; }
        public string TokenEndpoint { get; set; }
        public string Authority { get; set; }
    }
}
