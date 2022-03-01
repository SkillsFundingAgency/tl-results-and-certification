using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess
{
    public class ResultCsvRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public int RowNum { get; set; }
        public long Uln { get; set; }
        public string CoreCode { get; set; }
        public string CoreAssessmentSeries { get; set; }
        public string CoreGrade { get; set; }
        public IList<string> SpecialismCodes { get; set; }
        public string SpecialismAssessmentSeries { get; set; }
        public IList<string> SpecialismGrades { get; set; }
    }
}
