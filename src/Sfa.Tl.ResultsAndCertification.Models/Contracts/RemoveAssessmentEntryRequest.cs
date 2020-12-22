using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class RemoveAssessmentEntryRequest
    {
        public int AssessmentId { get; set; }

        public long AoUkprn { get; set; }

        public AssessmentEntryType AssessmentEntryType { get; set; }

        public string PerformedBy { get; set; }
    }
}