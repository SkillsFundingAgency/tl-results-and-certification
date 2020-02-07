using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class AwardingOrganisation : BaseModel
    {
        public AwardingOrganisation()
        {
            AwardingOrganisationPathwayStatus = new List<AwardingOrganisationPathwayStatus>();
        }

        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public virtual IEnumerable<AwardingOrganisationPathwayStatus> AwardingOrganisationPathwayStatus { get; set; }

    }
}