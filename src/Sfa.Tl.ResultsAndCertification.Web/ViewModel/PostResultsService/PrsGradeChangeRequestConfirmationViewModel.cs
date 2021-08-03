using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsGradeChangeRequestConfirmationViewModel
    {
        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public PrsGradeChangeConfirmationNavigationOptions? NavigationOption { get; set; }
    }
}
