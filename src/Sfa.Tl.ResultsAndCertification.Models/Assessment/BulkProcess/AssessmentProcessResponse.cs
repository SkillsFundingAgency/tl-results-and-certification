using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess
{
    public class AssessmentProcessResponse : ValidationState<BulkProcessValidationError>
    {
        public bool IsSuccess { get; set; }

        public BulkUploadStats BulkUploadStats { get; set; }
    }
}
