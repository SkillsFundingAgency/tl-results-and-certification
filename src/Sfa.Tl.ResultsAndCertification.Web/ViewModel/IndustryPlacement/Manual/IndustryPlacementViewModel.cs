﻿namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual
{
    public class IndustryPlacementViewModel
    {
        public IpCompletionViewModel IpCompletion { get; set; }
        public SpecialConsiderationViewModel SpecialConsideration { get; set; }

        public bool IsChangeModeAllowed { get; set; }

        public void ResetChangeMode()
        {
            IpCompletion.IsChangeMode = false;

            if (SpecialConsideration != null)
            {
                SpecialConsideration.Hours.IsChangeMode = false;
                SpecialConsideration.Reasons.IsChangeMode = false;
            }
        }
    }
}