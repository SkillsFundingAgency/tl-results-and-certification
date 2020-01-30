using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlSpecialism : BaseEntity
    {
        public TlSpecialism()
        {
            TlPathwaySpecialismCombinations = new HashSet<TlPathwaySpecialismCombination>();
            TlPathwaySpecialismMars = new HashSet<TlPathwaySpecialismMar>();
            TqProviders = new HashSet<TqProvider>();
        }

        public int PathwayId { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }

        public virtual TlPathway Pathway { get; set; }
        public virtual ICollection<TlPathwaySpecialismCombination> TlPathwaySpecialismCombinations { get; set; }
        public virtual ICollection<TlPathwaySpecialismMar> TlPathwaySpecialismMars { get; set; }
        public virtual ICollection<TqProvider> TqProviders { get; set; }
    }
}
