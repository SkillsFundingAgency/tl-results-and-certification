namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AwardingOrganisation
    {
        public int Id { get; set; }
        public long Ukprn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
