using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess
{
    public class ResultProcessResponse
    {
        public bool IsSuccess { get; set; }
        public BulkUploadStats BulkUploadStats { get; set; }
    }
}
