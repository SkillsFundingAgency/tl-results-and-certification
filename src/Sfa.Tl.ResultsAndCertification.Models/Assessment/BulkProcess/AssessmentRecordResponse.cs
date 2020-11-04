using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess
{
    public class AssessmentRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public int TqRegistrationPathwayId { get; set; }
        public int? PathwayAssessmentSeriesId { get; set; }

        public int? TqRegistrationSpecialismId { get; set; }
        public int? SpecialismAssessmentSeriesId { get; set; }
    }
}
