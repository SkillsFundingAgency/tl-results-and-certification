﻿using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TqAwardingOrganisation : BaseEntity
    {
        public TqAwardingOrganisation()
        {
            IsActive = true;
            TqProviders = new HashSet<TqProvider>();
        }

        public int TlAwardingOrganisatonId { get; set; }
        public int TlPathwayId { get; set; }
        public int ReviewStatus { get; set; }
        public bool IsActive { get; set; }

        public virtual TlAwardingOrganisation TlAwardingOrganisaton { get; set; }
        public virtual TlPathway TlPathway { get; set; }

        public virtual ICollection<TqProvider> TqProviders { get; set; }
    }
}