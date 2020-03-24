namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class SelectProviderTlevel
    {
        public int TqAwardingOrganisationId { get; set; }
        public int ProviderId { get; set; }
        public int PathwayId { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
    }
}
