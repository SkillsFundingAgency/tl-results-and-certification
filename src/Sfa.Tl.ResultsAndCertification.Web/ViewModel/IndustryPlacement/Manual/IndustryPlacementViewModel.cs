namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IndustryPlacementViewModel
    {
        public IpCompletionViewModel IpCompletion { get; set; }
        public IpModelViewModel IpModelViewModel { get; set; }
        public SpecialConsiderationViewModel SpecialConsideration { get; set; }
        public IpTempFlexibilityViewModel TempFlexibility { get; set; }

        public bool IsChangeModeAllowed { get; set; }
    }
}