namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlLookup : BaseEntity
    {
        public string Category { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}
