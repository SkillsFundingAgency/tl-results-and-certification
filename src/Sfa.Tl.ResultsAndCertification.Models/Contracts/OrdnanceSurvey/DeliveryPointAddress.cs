using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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
                
        [JsonProperty("sub_building_name")]
        public string SubBuildingName { get; set; }

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

        [JsonIgnore]
        public string FormattedBuildingName => string.Join(", ", SubBuildingAndBuildingName);
        
        [JsonIgnore]
        public string FormattedBuildingNumberAndThroughfare => string.Join(", ", BuildingNumberAndThroughfare);
        
        [JsonIgnore]
        public IEnumerable<string> SubBuildingAndBuildingName => new string[] { SubBuildingName, BuildingName }.Where(x => !string.IsNullOrWhiteSpace(x));

        [JsonIgnore]
        public IEnumerable<string> BuildingNumberAndThroughfare => new string[] { BuildingNumber, ThroughfareName }.Where(x => !string.IsNullOrWhiteSpace(x));
    }
}