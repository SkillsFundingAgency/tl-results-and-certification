using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ResultDetails
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }

        // Pathway Assessment
        public string PathwayLarId { get; set; }
        public string PathwayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }

        // Specialism Assessment
        public string SpecialismLarId { get; set; }
        public string SpecialismName { get; set; }

        // Pathway Result
        public string PathwayResult { get; set; }
        public int? PathwayResultId { get; set; }

        public RegistrationPathwayStatus Status { get; set; }
    }
}
