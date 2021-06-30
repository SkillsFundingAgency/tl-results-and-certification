using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class PrsLearnerDetails
    {
        // Registration
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public RegistrationPathwayStatus Status { get; set; }

        // Core Component 
        public string TlevelTitle { get; set; }
        public string PathwayName { get; set; }
        public string PathwayCode { get; set; }

        public int PathwayAssessmentId { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public int PathwayResultId { get; set; }
        public string PathwayGrade { get; set; }
        public DateTime PathwayGradeLastUpdatedOn { get; set; }
        public string PathwayGradeLastUpdatedBy { get; set; }
    }
}
