namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AwardingOrganisationPathwayStatus : BaseModel
    {
        public int TlAwardingOrganisatonId { get; set; }
        public int TlRouteId { get; set; }
        public int TlPathwayId { get; set; }
        public int ReviewStatus { get; set; }

        public Pathway Pathway { get; set; }
        public Route Route { get; set; }
        public AwardingOrganisation AwardingOrganisaton { get; set; }
    }
}