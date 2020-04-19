using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqAwardingOrganisation : BaseEntity
    {
        public TqAwardingOrganisation()
        {
            TqProviders = new HashSet<TqProvider>();
        }

        public int TlAwardingOrganisatonId { get; set; }
        public int TlRouteId { get; set; }
        public int TlPathwayId { get; set; }
        public int ReviewStatus { get; set; }
        public bool IsActive { get; set; }

        public virtual TlAwardingOrganisation TlAwardingOrganisaton { get; set; }
        public virtual TlPathway TlPathway { get; set; }
        public virtual TlRoute TlRoute { get; set; }

        public virtual ICollection<TqProvider> TqProviders { get; set; }

        public int Count()
        {
            throw new NotImplementedException();
        }
    }
}
