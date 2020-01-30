using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class Provider : BaseEntity
    {
        public Provider()
        {
            TqProviders = new HashSet<TqProvider>();
        }

        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsTlevelProvider { get; set; } // TODO: do we need this flag?? we get this info fro TqProvider table anyway.

        public virtual ICollection<TqProvider> TqProviders { get; set; }
    }
}
