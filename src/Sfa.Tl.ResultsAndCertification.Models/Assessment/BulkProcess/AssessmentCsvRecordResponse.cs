using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess
{
    public class AssessmentCsvRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public int RowNum { get; set; }
        public long Uln { get; set; }
        public string CoreCode { get; set; }
        public string CoreAssessmentEntry { get; set; }
        public string SpecialismCode { get; set; }
        public string SpecialismAssessmentEntry { get; set; }
    }
}
