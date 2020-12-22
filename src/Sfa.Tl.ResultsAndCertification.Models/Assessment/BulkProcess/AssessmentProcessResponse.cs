using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess
{
    public class AssessmentProcessResponse
    {
        public bool IsSuccess { get; set; }

        public BulkUploadStats BulkUploadStats { get; set; }
    }
}
