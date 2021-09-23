namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public class TlPathwaySpecialismCombination : BaseEntity
    {
        public int TlPathwayId { get; set; }
        public int TlSpecialismId { get; set; }
        public int GroupId { get; set; }
        public bool IsActive { get; set; }
        
        public virtual TlPathway TlPathway { get; set; }
        public virtual TlSpecialism TlSpecialism { get; set; }
    }
}
