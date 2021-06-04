using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement
{
    public class SoaLearnerRecordDetails
    {
        //Learner's registration details
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }

        //Learner's technical qualification details
        public string TlevelTitle { get; set; }
        public string PathwayName { get; set; }
        public string PathwayCode { get; set; }
        public string PathwayGrade { get; set; }
        public string SpecialismName { get; set; }
        public string SpecialismCode { get; set; }
        public string SpecialismGrade { get; set; }

        //Learner's T level component achievements
        public bool IsEnglishAndMathsAchieved { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }
        public bool? IsSendLearner { get; set; }
        public IndustryPlacementStatus IndustryPlacementStatus { get; set; }

        // Provider Organisation's postal address
        public Address ProviderAddress { get; set; }

        // Validation properties
        public RegistrationPathwayStatus Status { get; set; }

        public bool HasPathwayResult => !string.IsNullOrWhiteSpace(PathwayGrade);
        public bool IsIndustryPlacementAdded => IndustryPlacementStatus != IndustryPlacementStatus.NotSpecified;
        public bool IsLearnerRegistered => Status == RegistrationPathwayStatus.Active || Status == RegistrationPathwayStatus.Withdrawn;
        public bool IsNotWithdrawn => Status == RegistrationPathwayStatus.Active;
        public bool IsIndustryPlacementCompleted => IndustryPlacementStatus == IndustryPlacementStatus.Completed || IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration;
    }
}
