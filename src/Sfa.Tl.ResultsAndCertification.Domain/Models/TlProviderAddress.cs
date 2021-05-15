namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlProviderAddress : BaseEntity
    {
        public int TlProviderId { get; set; }
        public string DepartmentName { get; set; }
        public string OrganisationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public bool IsActive { get; set; }

        public virtual TlProvider TlProvider { get; set; }
    }
}