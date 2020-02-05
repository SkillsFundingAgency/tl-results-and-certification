using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlPathway : BaseEntity
    {
        public TlPathway()
        {
            TlPathwaySpecialismCombinations = new HashSet<TlPathwaySpecialismCombination>();
            TlPathwaySpecialismMars = new HashSet<TlPathwaySpecialismMar>();
            TlProviders = new HashSet<TlProvider>();
            TlSpecialisms = new HashSet<TlSpecialism>();
            TqAwardingOrganisations = new HashSet<TqAwardingOrganisation>();
        }

        public int TlRouteId { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }

        public virtual TlRoute TlRoute { get; set; }
        public virtual ICollection<TlPathwaySpecialismCombination> TlPathwaySpecialismCombinations { get; set; }
        public virtual ICollection<TlPathwaySpecialismMar> TlPathwaySpecialismMars { get; set; }
        public virtual ICollection<TlProvider> TlProviders { get; set; }
        public virtual ICollection<TlSpecialism> TlSpecialisms { get; set; }
        public virtual ICollection<TqAwardingOrganisation> TqAwardingOrganisations { get; set; }
    }
}
