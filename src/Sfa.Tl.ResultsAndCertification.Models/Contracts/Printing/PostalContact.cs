namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing
{
    public class PostalContact
    {
        public string DepartmentName { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public string UKPRN { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public int CertificateCount { get; set; }
    }
}
