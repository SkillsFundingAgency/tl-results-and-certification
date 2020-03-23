namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Provider.SelectProviderTlevels
{
    public class SelectProviderTlevelViewModel
    {
        public int TqAwardingOrganisationId { get; set; }
        public int ProviderId { get; set; }
        public int PathwayId { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
        public string TlevelTitle => $"{RouteName}: {PathwayName}";
        public bool IsSelected { get; set; }
    }
}
