using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.OverallResults
{
    public class OverallResultDetail
    {
        public string TlevelTitle { get; set; }
        public string PathwayName { get; set; }
        public string PathwayLarId { get; set; }
        public string PathwayResult { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public List<OverallSpecialismDetail> SpecialismDetails { get; set; }
        public string IndustryPlacementStatus { get; set; }
        public string OverallResult { get; set; }
    }
}
