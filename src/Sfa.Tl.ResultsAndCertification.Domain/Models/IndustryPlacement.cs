using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class IndustryPlacement : BaseEntity
    {
        public int TqRegistrationPathwayId { get; set; }
        public IndustryPlacementStatus Status { get; set; }
        public string Details { get; set; }

        public virtual TqRegistrationPathway TqRegistrationPathway { get; set; }
    }
}