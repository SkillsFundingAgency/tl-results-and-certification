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

        public bool IsLearnerRegistered { get; set; } // TODO: Delete?
        public bool IsLearnerRecordAdded { get; set; } // TODO: Delete?
        public bool IsEnglishAndMathsAchieved { get; set; } // TODO: Delete?
        public bool HasLrsEnglishAndMaths { get; set; } // TODO: Delete?
        public bool? IsSendLearner { get; set; } // TODO: Delete?

        // English and Maths
        public SubjectStatus IsEnglishAchieved { get; set; }
        public SubjectStatus IsMathsAchieved { get; set; }

        // Industry placement
        public int IndustryPlacementId { get; set; }
        public IndustryPlacementStatus? IndustryPlacementStatus { get; set; }
    }
}