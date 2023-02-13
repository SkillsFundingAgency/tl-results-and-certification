using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess
{
    public class IndustryPlacementRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public IndustryPlacementRecordResponse()
        {
            SpecialConsiderationReasons = new List<int?>();
        }

        public int? TqRegistrationPathwayId { get; set; }
        public int? IpStatus { get; set; } // TODO: should be non-nullable
        public int? IpHours { get; set; }

        public List<int?> SpecialConsiderationReasons { get; set; }
    }
}
