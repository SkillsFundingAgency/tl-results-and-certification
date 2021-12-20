namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class LrsLearnerVerificationAndLearningEventsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public int TotalCount { get; set; }

        public int LrsCount { get; set; }

        public int ModifiedCount { get; set; }

        public int SavedCount { get; set; }
    }
}