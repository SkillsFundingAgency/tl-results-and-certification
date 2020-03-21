using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ProviderTlevels : BaseModel
    {
        //public int TqAwardingOrganisationId { get; set; }
        public int TlProviderId { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }
        public IEnumerable<SelectProviderTlevel> Tlevels { get; set; }
    }
}
