using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess
{
    public class IndustryPlacementProcessResponse : ValidationState<BulkProcessValidationError>
    {
        public bool IsSuccess { get; set; }
        public BulkUploadStats BulkUploadStats { get; set; }
    }
}
