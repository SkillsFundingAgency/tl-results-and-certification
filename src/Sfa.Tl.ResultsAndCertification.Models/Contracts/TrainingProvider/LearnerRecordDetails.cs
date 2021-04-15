using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class LearnerRecordDetails
    {
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public string PathwayName { get; set; }
        public bool IsLearnerRegistered { get; set; }
        public bool IsLearnerRecordAdded { get; set; }
        public bool IsEnglishAndMathsAchieved { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }
        public bool? IsSendLearner { get; set; }
        public int IndustryPlacementId { get; set; }
        public IndustryPlacementStatus? IndustryPlacementStatus { get; set; }
    }
}