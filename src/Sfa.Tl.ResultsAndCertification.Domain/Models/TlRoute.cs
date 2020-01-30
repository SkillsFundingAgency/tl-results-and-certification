using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlRoute : BaseEntity
    {
        public TlRoute()
        {
            TlPathways = new HashSet<TlPathway>();
            TqProviders = new HashSet<TqProvider>();
        }

        public string Name { get; set; }

        public virtual ICollection<TlPathway> TlPathways { get; set; }
        public virtual ICollection<TqProvider> TqProviders { get; set; }
    }
}
