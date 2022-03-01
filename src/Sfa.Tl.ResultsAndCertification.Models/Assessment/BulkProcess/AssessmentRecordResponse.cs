using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess
{
    public class AssessmentRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public AssessmentRecordResponse()
        {
            TqRegistrationSpecialismIds = new List<int>();
        }

        public int? TqRegistrationPathwayId { get; set; }
        public int? PathwayAssessmentSeriesId { get; set; }
        
        public int? SpecialismAssessmentSeriesId { get; set; }
        public IEnumerable<int> TqRegistrationSpecialismIds { get; set; }

    }
}
