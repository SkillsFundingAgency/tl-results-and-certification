namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class AppealGradeAfterDeadlineRequest
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public string RequestedMessage { get; set; }
        public string RequestedUserEmailAddress { get; set; }
    }
}
