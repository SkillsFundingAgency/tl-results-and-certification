using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.PostResultsService
{
    public class RommsRecordResponse : ValidationState<BulkProcessValidationError>
    {

        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public bool OpenCoreRomm { get; set; }
        public bool AddCoreRommOutcome { get; set; }
        public string CoreRommOutcome { get; set; }
    }
}
