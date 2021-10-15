using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas
{
    public class UcasError
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("rejected")]
        public string Rejected { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
