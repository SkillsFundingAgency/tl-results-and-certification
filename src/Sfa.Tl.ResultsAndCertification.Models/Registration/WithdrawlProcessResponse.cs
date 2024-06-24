using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration
{
    public class WithdrawlProcessResponse : ValidationState<BulkProcessValidationError>
    {
        public bool IsSuccess { get; set; }

        public BulkUploadStats BulkUploadStats { get; set; }
    }
}
