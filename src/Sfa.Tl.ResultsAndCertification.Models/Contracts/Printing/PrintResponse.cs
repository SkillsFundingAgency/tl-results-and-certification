using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class PrintResponse
    {
        [JsonProperty("PrintRequestResponse")]
        public PrintRequestResponse PrintRequestResponse { get; set; }
    }
}
