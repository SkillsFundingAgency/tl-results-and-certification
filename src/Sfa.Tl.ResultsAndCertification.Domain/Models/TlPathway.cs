using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlPathway : BaseEntity
    {
        public TlPathway()
        {
            TlPathwaySpecialismCombination = new HashSet<TlPathwaySpecialismCombination>();
            TlPathwaySpecialismMar = new HashSet<TlPathwaySpecialismMar>();
            TlSpecialism = new HashSet<TlSpecialism>();
            TqProvider = new HashSet<TqProvider>();
        }

        public int RouteId { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }

        public virtual TlRoute Route { get; set; }
        public virtual ICollection<TlPathwaySpecialismCombination> TlPathwaySpecialismCombination { get; set; }
        public virtual ICollection<TlPathwaySpecialismMar> TlPathwaySpecialismMar { get; set; }
        public virtual ICollection<TlSpecialism> TlSpecialism { get; set; }
        public virtual ICollection<TqProvider> TqProvider { get; set; }
    }
}
