
namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class IpAchieved : BaseEntity
    {
        public int IndustryPlacementId { get; set; }
        public int IpLookupId { get; set; }
        public bool IsActive { get; set; }

        public virtual IndustryPlacement IndustryPlacement { get; set; }
        public virtual IpLookup IpLookup { get; set; }
    }
}
