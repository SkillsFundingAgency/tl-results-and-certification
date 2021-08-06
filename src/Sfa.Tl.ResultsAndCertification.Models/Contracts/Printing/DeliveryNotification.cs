using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class DeliveryNotification
    {
        public int BatchNumber { get; set; }
        public List<TrackingDetail> TrackingDetails { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
