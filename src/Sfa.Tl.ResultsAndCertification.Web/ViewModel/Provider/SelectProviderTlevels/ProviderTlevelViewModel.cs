﻿namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels
{
    public class ProviderTlevelViewModel
    {
        public int? TqProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public int TlProviderId { get; set; }
        public string TlevelTitle { get; set; }
        public bool IsSelected { get; set; }
    }
}
