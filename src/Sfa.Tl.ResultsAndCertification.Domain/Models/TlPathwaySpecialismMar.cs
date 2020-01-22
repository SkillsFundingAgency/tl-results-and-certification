namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlPathwaySpecialismMar : BaseEntity
    {
        public int MarId { get; set; }
        public int? PathwayId { get; set; }
        public int? SpecialismId { get; set; }

        public virtual TlPathway Pathway { get; set; }
        public virtual TlSpecialism Specialism { get; set; }
    }
}
