using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey
{
    public class DeliveryPointAddress
    {
        [JsonProperty("uprn")]
        public string Uprn { get; set; }

        [JsonProperty("address")]
        public string FormattedAddress { get; set; }
        
        [JsonProperty("building_number")]
        public string AddressLine1 { get; set; }
        
        [JsonProperty("thoroughfare_name")]
        public string AddressLine2 { get; set; }
        
        [JsonProperty("post_town")]
        public string Town { get; set; }
        
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
    }
}
