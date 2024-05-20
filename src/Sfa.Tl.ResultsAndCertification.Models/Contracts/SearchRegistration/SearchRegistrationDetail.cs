namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.SearchRegistration
{
    public class SearchRegistrationDetail
    {
        public int RegistrationProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string PathwayName { get; set; }
        public string PathwayLarId { get; set; }
        public int AcademicYear { get; set; }
        public bool IsWithdrawn { get; set; }
        public bool HasResults { get; set; }
    }
}