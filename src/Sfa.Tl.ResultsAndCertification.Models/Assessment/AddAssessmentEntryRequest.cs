using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Assessment
{
    public class AddAssessmentEntryRequest
    {
        public long AoUkprn { get; set; }
        public int ProfileId { get; set; }
        public int AssessmentSeriesId { get; set; }
        public AssessmentEntryType AssessmentEntryType { get; set; }
        public string PerformedBy { get; set; }
    }
}
