namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ProviderTlevel
    {
        public int? TqProviderId { get; set; }
        public int TqAwardingOrganisationId { get; set; }
        public int TlProviderId { get; set; }
        //public int PathwayId { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
        public string CreatedBy { get; set; }
    }

}
