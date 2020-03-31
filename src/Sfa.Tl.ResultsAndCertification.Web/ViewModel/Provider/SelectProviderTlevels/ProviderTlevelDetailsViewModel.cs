namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels
{
    public class ProviderTlevelDetailsViewModel
    {
        public int? TqProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public int ProviderId { get; set; }
        public int PathwayId { get; set; }
        public string TlevelTitle { get; set; }
        public bool IsSelected { get; set; }
    }
}
