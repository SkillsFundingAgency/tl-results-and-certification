using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess
{
    public class ResultCsvRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public int RowNum { get; set; }
        public long Uln { get; set; }
        public string CoreCode { get; set; }
        public string CoreAssessmentSeries { get; set; }
        public string CoreGrade { get; set; }
    }
}
