using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ProviderTlevels : BaseModel
    {
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }
        public IEnumerable<AwardingOrganisationPathwayStatus> Tlevels { get; set; }
    }
}
