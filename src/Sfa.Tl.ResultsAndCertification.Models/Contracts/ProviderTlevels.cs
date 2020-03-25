using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ProviderTlevels : BaseModel
    {
        public int ProviderId { get; set; }
        public string DisplayName { get; set; }
        public long Ukprn { get; set; }
        public IEnumerable<ProviderTlevelDetails> Tlevels { get; set; }
    }
}
