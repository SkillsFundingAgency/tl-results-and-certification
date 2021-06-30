namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress
{
    public class AddAddressRequest
    {
        public long Ukprn { get; set; }
        public string DepartmentName { get; set; }
        public string OrganisationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string PerformedBy { get; set; }
    }
}
