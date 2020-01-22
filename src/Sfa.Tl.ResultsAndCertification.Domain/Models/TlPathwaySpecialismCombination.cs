namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlPathwaySpecialismCombination : BaseEntity
    {
        public int PathwayId { get; set; }
        public int SpecialismId { get; set; }
        public string Group { get; set; }

        public virtual TlPathway Pathway { get; set; }
        public virtual TlSpecialism Specialism { get; set; }
    }
}
