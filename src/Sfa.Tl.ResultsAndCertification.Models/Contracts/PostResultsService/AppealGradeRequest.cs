using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class AppealGradeRequest
    {
        public long AoUkprn { get; set; }
        public int ProfileId { get; set; }
        public int AssessentId { get; set; }
        public int ResultId { get; set; }
        public int ResultLookupId { get; set; }
        public ComponentType ComponentType { get; set; }
        public PrsStatus PrsStatus { get; set; }
        public string PerformedBy { get; set; }
    }
}