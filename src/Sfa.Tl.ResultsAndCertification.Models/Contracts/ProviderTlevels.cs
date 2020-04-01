using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ProviderTlevels : ProviderDetails
    {
        //public int ProviderId { get; set; }
        public IEnumerable<ProviderTlevelDetails> Tlevels { get; set; }
    }
}
