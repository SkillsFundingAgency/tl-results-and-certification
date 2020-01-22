using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlSpecialism : BaseEntity
    {
        public TlSpecialism()
        {
            TlPathwaySpecialismCombination = new HashSet<TlPathwaySpecialismCombination>();
            TlPathwaySpecialismMar = new HashSet<TlPathwaySpecialismMar>();
            TqProvider = new HashSet<TqProvider>();
        }

        public int PathwayId { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }

        public virtual TlPathway Pathway { get; set; }
        public virtual ICollection<TlPathwaySpecialismCombination> TlPathwaySpecialismCombination { get; set; }
        public virtual ICollection<TlPathwaySpecialismMar> TlPathwaySpecialismMar { get; set; }
        public virtual ICollection<TqProvider> TqProvider { get; set; }
    }
}
