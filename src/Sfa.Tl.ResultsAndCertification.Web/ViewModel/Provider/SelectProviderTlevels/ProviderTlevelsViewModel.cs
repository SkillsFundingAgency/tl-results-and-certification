namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels
{
    public class ProviderTlevelsViewModel
    {
        public int TqAwardingOrganisationId { get; set; }
        public int TlProviderId { get; set; }
        public int TlPathwayId { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
        public string TlevelTitle => $"{RouteName}: {PathwayName}";
        public bool IsSelected { get; set; }
    }
}
