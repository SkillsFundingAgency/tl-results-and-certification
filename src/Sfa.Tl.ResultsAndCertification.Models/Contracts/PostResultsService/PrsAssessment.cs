namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class PrsAssessment
    {
        public int AssessmentId { get; set; }
        public string SeriesName { get; set; }
        public bool HasResult { get; set; }
    }
}
