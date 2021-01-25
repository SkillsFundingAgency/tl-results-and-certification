namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class CoreResult
    {
        public int ProfileId { get; set; }
        public int? AssessmentId { get; set; }
        public string PathwayLarId { get; set; }
        public string PathwayName { get; set; }
        public string AssessmentSeries { get; set; }

        // Result
        public int? ResultId { get; set; }
        public string ResultCode { get; set; }
    }
}
