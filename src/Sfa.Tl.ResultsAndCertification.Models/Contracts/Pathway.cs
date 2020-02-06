using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class Pathway
    {
        public Pathway()
        {
            AwardingOrganisations = new List<AwardingOrganisationPathwayStatus>();
        }

        public int RouteId { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }

        public virtual Route TlRoute { get; set; }
        public virtual IEnumerable<AwardingOrganisationPathwayStatus> AwardingOrganisations { get; set; }
    }
}