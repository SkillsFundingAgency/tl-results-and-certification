using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlMandatoryAdditionalRequirement : BaseEntity
    {
        public TlMandatoryAdditionalRequirement()
        {
            TlPathwaySpecialismMars = new HashSet<TlPathwaySpecialismMar>();
        }

        public string Name { get; set; }
        public bool IsRegulatedQualification { get; set; }
        public string LarId { get; set; }
        public virtual ICollection<TlPathwaySpecialismMar> TlPathwaySpecialismMars { get; set; }
    }
}
