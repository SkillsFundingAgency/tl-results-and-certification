using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AssessmentDetailsViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }

        public long ProviderDisplayName { get; set; }
        public string PathwayDisplayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }


        public string SpecialismDisplayName { get; set; }
        public string SpecialismAssessmentSeries { get; set; }

        public RegistrationPathwayStatus PathwayStatus { get; set; }
    }
}
