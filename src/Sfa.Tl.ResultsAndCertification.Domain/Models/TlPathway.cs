using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlPathway : BaseEntity
    {
        public TlPathway()
        {
            TlPathwaySpecialismCombinations = new HashSet<TlPathwaySpecialismCombination>();
            TlPathwaySpecialismMars = new HashSet<TlPathwaySpecialismMar>();
            TlSpecialisms = new HashSet<TlSpecialism>();
            TqProviders = new HashSet<TqProvider>();
        }

        public int RouteId { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }

        public virtual TlRoute Route { get; set; }
        public virtual ICollection<TlPathwaySpecialismCombination> TlPathwaySpecialismCombinations { get; set; }
        public virtual ICollection<TlPathwaySpecialismMar> TlPathwaySpecialismMars { get; set; }
        public virtual ICollection<TlSpecialism> TlSpecialisms { get; set; }
        public virtual ICollection<TqProvider> TqProviders { get; set; }
    }
}
