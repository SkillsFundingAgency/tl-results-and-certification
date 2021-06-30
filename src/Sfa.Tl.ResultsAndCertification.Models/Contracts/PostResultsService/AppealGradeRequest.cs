using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class AppealGradeRequest
    {
        public long AoUkprn { get; set; }
        public int ProfileId { get; set; } // TODO: required? I don't think so. 
        public int AssessentId { get; set; } // TODO: required?
        public int ResultId { get; set; }
        public ComponentType ComponentType { get; set; }
        public PrsStatus PrsStatus { get; set; }
        public string PerformedBy { get; set; }
    }
}