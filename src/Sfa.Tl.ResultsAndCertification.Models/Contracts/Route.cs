using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class Route
    {
        public Route()
        {
            Pathways = new List<Pathway>();
            AwardingOrganisations = new List<AwardingOrganisationPathwayStatus>();
        }

        public string Name { get; set; }

        public IEnumerable<Pathway> Pathways { get; set; }
        public IEnumerable<AwardingOrganisationPathwayStatus> AwardingOrganisations { get; set; }
    }
}