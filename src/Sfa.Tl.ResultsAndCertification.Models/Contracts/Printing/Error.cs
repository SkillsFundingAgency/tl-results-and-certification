using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class Error
    {
        [JsonProperty("CertificateNumber")]
        public string CertificateNumber { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("ErrorMessage")]
        public string ErrorMessage { get; set; }
    }
}
