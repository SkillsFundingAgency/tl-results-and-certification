using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlSpecialism : BaseEntity
    {
        public TlSpecialism()
        {
            TqSpecialismRegistrations = new HashSet<TqRegistrationSpecialism>();
        }

        public int TlPathwayId { get; set; }
        public string LarId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual TlPathway TlPathway { get; set; }
        public virtual ICollection<TqRegistrationSpecialism> TqSpecialismRegistrations { get; set; }
    }
}
