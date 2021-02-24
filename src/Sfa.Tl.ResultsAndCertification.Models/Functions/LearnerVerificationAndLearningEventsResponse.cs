namespace Sfa.Tl.ResultsAndCertification.Models.Functions
{
    public class LearnerVerificationAndLearningEventsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public int RegistrationsRecordsCount { get; set; }

        public int LrsRecordsCount { get; set; }

        public int ModifiedRecordsCount { get; set; }

        public int SavedRecordsCount { get; set; }        
    }
}
