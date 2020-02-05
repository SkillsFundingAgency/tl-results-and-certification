namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlPathwaySpecialismCombination : BaseEntity
    {
        public int TlPathwayId { get; set; }
        public int TlSpecialismId { get; set; }
        public string Group { get; set; }

        public virtual TlPathway TlPathway { get; set; }
        public virtual TlSpecialism TlSpecialism { get; set; }
    }
}
