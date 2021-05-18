using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey
{
    public class PostcodeLookupResult
    {        
        [JsonProperty("results")]
        public List<AddressResult> AddressResult { get; set; }
    }
}