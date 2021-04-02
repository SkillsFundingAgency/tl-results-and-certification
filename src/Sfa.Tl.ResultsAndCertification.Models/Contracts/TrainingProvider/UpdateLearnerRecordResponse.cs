namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class UpdateLearnerRecordResponse
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public bool IsModified { get; set; }
        public bool IsSuccess { get; set; }
    }
}