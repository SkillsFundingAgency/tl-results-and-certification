using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AssessmentDetails
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }        
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }

        public string PathwayLarId { get; set; }
        public string PathwayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public int? PathwayAssessmentId { get; set; }
        public bool IsCoreEntryEligible { get; set; }

        public string SpecialismLarId { get; set; }
        public string SpecialismName { get; set; }
        public string SpecialismAssessmentSeries { get; set; }
        public int? SpecialismAssessmentId { get; set; }

        public int? PathwayResultId { get; set; }
        public bool HasAnyOutstandingPathwayPrsActivities  { get; set; }
        public bool IsIndustryPlacementExist { get; set; }

        public RegistrationPathwayStatus Status { get; set; }
    }
}
