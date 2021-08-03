using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class PrsGradeChangeRequest
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public string ReferenceNumber { get; set; } = Guid.NewGuid().ToString();
        public string RequestedMessage { get; set; }
        public string RequestedUserEmailAddress { get; set; }
    }
}
