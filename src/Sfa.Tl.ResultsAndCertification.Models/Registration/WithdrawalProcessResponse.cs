using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration
{
    public class WithdrawalProcessResponse : ValidationState<BulkProcessValidationError>
    {
        public bool IsSuccess { get; set; }

        public BulkUploadStats BulkUploadStats { get; set; }
    }
}
