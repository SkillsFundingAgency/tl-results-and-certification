using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey
{
    public class PostcodeLookupResult
    {
        [JsonIgnore]
        public int TotalResultsCount { get { return AddressResult != null ? AddressResult.Count : 0; } }
        
        [JsonProperty("results")]
        public List<AddressResult> AddressResult { get; set; }
    }
}
