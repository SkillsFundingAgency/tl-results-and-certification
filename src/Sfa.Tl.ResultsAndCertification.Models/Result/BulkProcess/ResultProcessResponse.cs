using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess
{
    public class ResultProcessResponse : ValidationState<BulkProcessValidationError>
    {
        public bool IsSuccess { get; set; }
        public BulkUploadStats BulkUploadStats { get; set; }
    }
}
