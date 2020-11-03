using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess
{
    public class AssessmentCsvRecordResponse : ValidationState<BulkProcessValidationError>
    {
        // TODO:
        public int RowNum { get; set; }

        public long Uln { get; set; }
    }
}
