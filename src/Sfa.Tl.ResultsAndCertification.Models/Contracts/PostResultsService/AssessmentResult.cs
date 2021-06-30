using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class AssessmentResult
    {
        public int? PathwayAssessmentId { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public int? PathwayResultId { get; set; }
        public string PathwayGrade { get; set; }
        public DateTime? PathwayGradeLastUpdatedOn { get; set; }
        public string PathwayGradeLastUpdatedBy { get; set; }

        public bool HasPathwayResult => !string.IsNullOrWhiteSpace(PathwayGrade);
    }
}