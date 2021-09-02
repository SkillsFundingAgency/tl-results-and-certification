namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Tlevels
{
    public class TlevelConfirmationViewModel
    {
        public int PathwayId { get; set; }
        public string TlevelConfirmationText { get; set; }
        public string TlevelTitle { get; set; }
        public bool IsQueried { get; set; }
        public bool ShowMoreTlevelsToReview { get; set; }
    }
}
