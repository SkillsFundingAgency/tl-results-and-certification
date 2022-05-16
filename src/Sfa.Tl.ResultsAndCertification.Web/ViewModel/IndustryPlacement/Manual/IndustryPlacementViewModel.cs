namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IndustryPlacementViewModel
    {
        public IpCompletionViewModel IpCompletion { get; set; }
        public IpModelViewModel IpModelViewModel { get; set; }
        public SpecialConsiderationViewModel SpecialConsideration { get; set; }
        public IpTempFlexibilityViewModel TempFlexibility { get; set; }

        public bool IsChangeModeAllowed { get; set; }


        public void ResetChangeMode()
        {
            IpCompletion.IsChangeMode = false;

            if (SpecialConsideration != null)
            {
                SpecialConsideration.Hours.IsChangeMode = false;
                SpecialConsideration.Reasons.IsChangeMode = false;
            }

            IpModelViewModel.IpModelUsed.IsChangeMode = false;

            if (IpModelViewModel.IpMultiEmployerUsed != null)
                IpModelViewModel.IpMultiEmployerUsed.IsChangeMode = false;

            if (IpModelViewModel.IpMultiEmployerOther != null)
                IpModelViewModel.IpMultiEmployerOther.IsChangeMode = false;

            if (IpModelViewModel.IpMultiEmployerSelect != null)
                IpModelViewModel.IpMultiEmployerSelect.IsChangeMode = false;

            if(TempFlexibility != null)
            {
                if(TempFlexibility.IpTempFlexibilityUsed != null)
                    TempFlexibility.IpTempFlexibilityUsed.IsChangeMode = false;

                if (TempFlexibility.IpBlendedPlacementUsed != null)
                    TempFlexibility.IpTempFlexibilityUsed.IsChangeMode = false;
            }
        }
    }
}