namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AwardingOrganisationPathwayStatus : BaseModel
    {
        public int PathwayId { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
        public int StatusId { get; set; }
    }
}