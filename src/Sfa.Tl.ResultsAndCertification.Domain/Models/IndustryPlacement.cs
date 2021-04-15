using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class IndustryPlacement : BaseEntity
    {
        public int TqRegistrationPathwayId { get; set; }
        public IndustryPlacementStatus Status { get; set; }

        public virtual TqRegistrationPathway TqRegistrationPathway { get; set; }
    }
}