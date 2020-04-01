using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ProviderTlevels : ProviderDetails
    {
        public IEnumerable<ProviderTlevel> Tlevels { get; set; }
    }
}
