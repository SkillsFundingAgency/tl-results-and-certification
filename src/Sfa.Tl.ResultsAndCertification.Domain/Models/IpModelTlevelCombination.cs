namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class IpModelTlevelCombination : BaseEntity
    {
        public int TlPathwayId { get; set; }
        public int IpLookupId { get; set; }
        public bool IsActive { get; set; }

        public virtual TlPathway TlPathway { get; set; }
        public virtual IpLookup IpLookup { get; set; }
    }
}
