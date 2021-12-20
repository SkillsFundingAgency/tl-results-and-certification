using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class RegistrationAssessmentDetails
    {       
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public RegistrationPathwayStatus PathwayStatus { get; set; }
        public bool IsCoreResultExist { get; set; }
        public bool HasAnyOutstandingPathwayPrsActivities { get; set; }
        public bool IsIndustryPlacementExist { get; set; }
    }
}