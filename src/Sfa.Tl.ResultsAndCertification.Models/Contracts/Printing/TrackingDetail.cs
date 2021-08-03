using Newtonsoft.Json;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class TrackingDetail
    {
        public string Name { get; set; }
        public string UKPRN { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }

        [JsonProperty("Signed for by")]
        public string SignedForBy { get; set; }

        [JsonProperty("Tracking ID")]
        public string TrackingID { get; set; }
        public DateTime? StatusChangeDate { get; set; }
    }
}
