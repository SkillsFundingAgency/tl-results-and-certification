using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement
{
    public class SoaLearnerRecordDetailsViewModel
    {
        //Learner's registration details
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }

        //Learner's technical qualification details
        public string TlevelTitle { get; set; }
        public string PathwayName { get; set; }
        public string PathwayGrade { get; set; }
        public string SpecialismName { get; set; }
        public string SpecialismGrade { get; set; }

        //Learner's T level component achievements
        public bool IsEnglishAndMathsAchieved { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }
        public bool? IsSendLearner { get; set; }
        public IndustryPlacementStatus IndustryPlacementStatus { get; set; }

        // Provider Organisation's postal address
        public AddressViewModel ProviderAddress { get; set; }

        // Validation properties
        public bool HasPathwayResult { get; set; }
        public bool IsIndustryPlacementAdded { get; set; }
        public bool IsLearnerRegistered { get; set; }
        public bool IsNotWithdrawn { get; set; }
        public bool IsIndustryPlacementCompleted { get; set; }
    }
}
