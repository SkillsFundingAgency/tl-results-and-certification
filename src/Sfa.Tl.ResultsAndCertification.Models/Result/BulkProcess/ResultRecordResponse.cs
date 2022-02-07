using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess
{
    public class ResultRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public ResultRecordResponse()
        {
            SpecialismAssessmentSeriesIds = new List<int>();
            SpecialismComponentGradeLookupIds = new List<int?>();
        }

        public int? TqPathwayAssessmentId { get; set; }
        public int? PathwayComponentGradeLookupId { get; set; }

        public IList<int> SpecialismAssessmentSeriesIds { get; set; }
        public IList<int?> SpecialismComponentGradeLookupIds { get; set; }
    }
}
