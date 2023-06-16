using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlDualSpecialismToSpecialism: BaseEntity
    {
        public int DualSpecialismId { get; set; }
        public int SpecialismId { get; set; }        

        public virtual TlDualSpecialism DualSpecialism { get; set; }
        public virtual TlSpecialism Specialism { get; set; }
    }
}
