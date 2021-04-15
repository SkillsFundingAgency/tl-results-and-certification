using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class FindLearnerRecord
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public bool IsLearnerRegistered { get; set; }
        public bool IsLearnerRecordAdded { get; set; }
        public bool IsEnglishAndMathsAchieved { get; set; }
        public bool HasSendQualification { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }
        public bool IsSendConfirmationRequired { get; set; }
    }
}
