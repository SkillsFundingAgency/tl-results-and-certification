using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration
{
    public class WithdrawlRecordResponse : ValidationState<BulkProcessValidationError>
    {

        public int ProfileId { get; set; }
        public long Uln { get; set; }
    }
}
