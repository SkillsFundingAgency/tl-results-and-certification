using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class LearnerRecordDetails
    {
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public int TlPathwayId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string TlevelTitle { get; set; }
        public int AcademicYear { get; set; }
        public string AwardingOrganisationName { get; set; }
        public SubjectStatus? MathsStatus { get;set; }
        public SubjectStatus? EnglishStatus { get; set; }

        public bool IsLearnerRegistered { get; set; }
        public RegistrationPathwayStatus RegistrationPathwayStatus { get; set; }
        public bool IsPendingWithdrawal { get; set; }
        // English and Maths
        public SubjectStatus IsEnglishAchieved { get; set; }
        public SubjectStatus IsMathsAchieved { get; set; }

        // Industry placement
        public int IndustryPlacementId { get; set; }
        public IndustryPlacementStatus? IndustryPlacementStatus { get; set; }
        public string IndustryPlacementDetails { get; set; }

        public string OverallResultDetails { get; set; }
        public DateTime? OverallResultPublishDate { get; set; }

        public int? PrintCertificateId { get; set; }
        public PrintCertificateType? PrintCertificateType { get; set; }
        public DateTime? LastDocumentRequestedDate { get; set; }
        public bool? IsReprint { get; set; }

        // Provider Organisation's postal address
        public Address ProviderAddress { get; set; }
    }
}