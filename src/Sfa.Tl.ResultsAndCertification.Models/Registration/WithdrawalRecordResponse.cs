using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration
{
    public class WithdrawalRecordResponse : ValidationState<BulkProcessValidationError>
    {

        public int ProfileId { get; set; }
        public long Uln { get; set; }
    }
}
