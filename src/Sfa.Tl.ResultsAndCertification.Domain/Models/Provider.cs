using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class Provider : BaseEntity
    {
        public Provider()
        {
            TqProvider = new HashSet<TqProvider>();
        }

        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsTlevelProvider { get; set; }
        public virtual ICollection<TqProvider> TqProvider { get; set; }
    }
}
