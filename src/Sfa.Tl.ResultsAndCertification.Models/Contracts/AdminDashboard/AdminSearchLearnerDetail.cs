namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminSearchLearnerDetail
    {
        public int RegistrationPathwayId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Provider { get; set; }
        public long ProviderUkprn { get; set; }
        public string AwardingOrganisation { get; set; }
        public int AcademicYear { get; set; }
    }
}