using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class AddResultViewModel
    {
        public int ProfileId { get; set; }
        public int TqPathwayAssessmentId { get; set; }
        public int TlLookupId { get; set; }

        public AssessmentEntryType AssessmentEntryType { get; set; }
    }
}