using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlAwardingOrganisation : BaseEntity
    {
        public TlAwardingOrganisation()
        {
            TqAwardingOrganisations = new HashSet<TqAwardingOrganisation>();
        }

        public string UkAon { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public virtual ICollection<TqAwardingOrganisation> TqAwardingOrganisations { get; set; }
    }
}
