using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlAwardingOrganisation : BaseEntity
    {
        public TlAwardingOrganisation()
        {
            TqAwardingOrganisations = new HashSet<TqAwardingOrganisation>();
        }

        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<TqAwardingOrganisation> TqAwardingOrganisations { get; set; }
    }
}
