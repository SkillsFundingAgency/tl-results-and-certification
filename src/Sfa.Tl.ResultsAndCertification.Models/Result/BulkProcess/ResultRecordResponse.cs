using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess
{
    public class ResultRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public int? TqPathwayAssessmentId { get; set; }
        public int? PathwayComponentGradeLookupId { get; set; }
    }
}
