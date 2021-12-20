using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess
{
    public class AssessmentCsvRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public AssessmentCsvRecordResponse()
        {
            SpecialismCodes = new List<string>();
        }

        public int RowNum { get; set; }
        public long Uln { get; set; }
        public string CoreCode { get; set; }
        public string CoreAssessmentEntry { get; set; }
        public IList<string> SpecialismCodes { get; set; }
        public string SpecialismAssessmentEntry { get; set; }
    }
}
