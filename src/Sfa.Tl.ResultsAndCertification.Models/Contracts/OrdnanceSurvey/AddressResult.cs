using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.OrdnanceSurvey
{
    public class AddressResult
    {
        [JsonProperty("dpa")]
        public DeliveryPointAddress DeliveryPointAddress { get; set; }
    }
}