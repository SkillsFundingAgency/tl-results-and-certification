using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
   public partial class TqProvider : BaseEntity
    {
        public int AwardingOrganisationId { get; set; } 
        public int ProviderId { get; set; }
        public int RouteId { get; set; }
        public int PathwayId { get; set; }
        public int SpecialismId { get; set; }

        public virtual TqAwardingOrganisation AwardingOrganisation { get; set; }
        public virtual TlPathway Pathway { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual TlRoute Route { get; set; }
        public virtual TlSpecialism Specialism { get; set; }  // TODO: Check if this link is required?
    }
}