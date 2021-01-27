using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AddResultRequest
    {
        public long AoUkprn { get; set; }
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int LookupId { get; set; }
        public AssessmentEntryType AssessmentEntryType { get; set; }
        public string PerformedBy { get; set; }
    }
}
