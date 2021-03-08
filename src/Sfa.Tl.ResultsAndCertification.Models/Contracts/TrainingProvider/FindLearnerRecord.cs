namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class FindLearnerRecord
    {
        public long Uln { get; set; }
        public string Name { get; set; }
        public string DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public bool IsLearnerRegistered { get; set; }
        public bool IsLearnerRecordAdded { get; set; }
        public bool IsSendQualification { get; set; }
    }
}
