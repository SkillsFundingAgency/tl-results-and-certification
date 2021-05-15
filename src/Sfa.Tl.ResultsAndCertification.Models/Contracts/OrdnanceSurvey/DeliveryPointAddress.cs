using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey
{
    public class DeliveryPointAddress
    {
        [JsonProperty("uprn")]
        public string Uprn { get; set; }

        [JsonProperty("address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("organisation_name")]
        public string OrganisationName { get; set; }

        [JsonProperty("building_name")]
        public string BuildingName { get; set; }

        [JsonProperty("building_number")]
        public string BuildingNumber { get; set; }
        
        [JsonProperty("thoroughfare_name")]
        public string ThroughfareName { get; set; }
        
        [JsonProperty("post_town")]
        public string Town { get; set; }
        
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
    }
}
