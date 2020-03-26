namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ProviderTlevelDetails
    {
        public int TqAwardingOrganisationId { get; set; }
        public int ProviderId { get; set; }
        public int PathwayId { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
        public string CreatedBy { get; set; }
    }

}
