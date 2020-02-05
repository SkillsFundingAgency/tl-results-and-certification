using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlRoute : BaseEntity
    {
        public TlRoute()
        {
            TlPathways = new HashSet<TlPathway>();
            TqAwardingOrganisations = new HashSet<TqAwardingOrganisation>();
        }

        public string Name { get; set; }

        public virtual ICollection<TlPathway> TlPathways { get; set; }
        public virtual ICollection<TqAwardingOrganisation> TqAwardingOrganisations { get; set; }
    }
}
