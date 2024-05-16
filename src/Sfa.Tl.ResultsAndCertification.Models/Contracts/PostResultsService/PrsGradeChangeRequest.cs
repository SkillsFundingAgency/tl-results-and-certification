namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class PrsGradeChangeRequest
    {
        public string LearnerName { get; set; }
        public long Uln { get; set; }
        public long ProviderUkprn { get; set; }
        public string RequestedMessage { get; set; }
        public string RequestedUserEmailAddress { get; set; }
    }
}
