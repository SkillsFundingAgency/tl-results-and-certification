using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlRoute : BaseEntity
    {
        public TlRoute()
        {
            TlPathway = new HashSet<TlPathway>();
            TqProvider = new HashSet<TqProvider>();
        }

        public string Name { get; set; }

        public virtual ICollection<TlPathway> TlPathway { get; set; }
        public virtual ICollection<TqProvider> TqProvider { get; set; }
    }
}
