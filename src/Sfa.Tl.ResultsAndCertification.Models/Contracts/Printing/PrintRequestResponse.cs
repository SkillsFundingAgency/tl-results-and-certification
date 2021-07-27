using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class PrintRequestResponse
    {
        [JsonProperty("BatchNumber")]
        public int BatchNumber { get; set; }
        [JsonProperty("Status")]
        public string Status { get; set; }
        [JsonProperty("Errors")]
        public List<Error> Errors { get; set; }
    }
}
