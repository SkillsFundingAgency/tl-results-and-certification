using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class IndustryPlacement : BaseEntity
    {
        public IndustryPlacement()
        {
            IpAchieved = new HashSet<IpAchieved>();
        }

        public int TqRegistrationPathwayId { get; set; }
        public IndustryPlacementStatus Status { get; set; }
        public int? Hours { get; set; }

        public virtual TqRegistrationPathway TqRegistrationPathway { get; set; }
        public virtual ICollection<IpAchieved> IpAchieved { get; set; }        
    }
}