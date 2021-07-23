using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ResultDetails
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }

        // Pathway Assessment
        public string TlevelTitle { get; set; }
        public string PathwayLarId { get; set; }
        public string PathwayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public int? PathwayAssessmentId { get; set; }
        public DateTime? AppealEndDate { get; set; }

        // Pathway Result
        public int? PathwayResultId { get; set; }
        public string PathwayResultCode { get; set; }
        public string PathwayResult { get; set; }

        public PrsStatus? PathwayPrsStatus { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
    }
}
