using System;
using System.Collections.Generic;
using System.Text;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqAwardingOrganisation : BaseEntity
    {
        public TqAwardingOrganisation()
        {
            TqProvider = new HashSet<TqProvider>();
        }

        public string UkAon { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public virtual ICollection<TqProvider> TqProvider { get; set; }
    }
}
