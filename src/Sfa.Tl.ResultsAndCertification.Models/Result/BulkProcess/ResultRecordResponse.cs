using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess
{
    public class ResultRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public ResultRecordResponse()
        {
            SpecialismResults = new Dictionary<int, int?>();
        }

        public int? TqPathwayAssessmentId { get; set; }
        public int? PathwayComponentGradeLookupId { get; set; }

        public Dictionary<int, int?> SpecialismResults { get; set; }
    }
}
