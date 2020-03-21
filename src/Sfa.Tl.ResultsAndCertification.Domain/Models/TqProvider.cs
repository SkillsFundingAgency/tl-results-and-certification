using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
   public partial class TqProvider : BaseEntity
    {
        public int TqAwardingOrganisationId { get; set; } 
        public int TlProviderId { get; set; }
        public int TlPathwayId { get; set; }
        public virtual TlProvider TlProvider { get; set; }
        public virtual TlPathway TlPathway { get; set; }
        public virtual TqAwardingOrganisation TqAwardingOrganisation { get; set; }
    }
}