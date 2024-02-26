using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewChangeIndustryPlacementRequest : ReviewChangeRequest
    {
        public ChangeIPDetails ChangeIPDetails { get; set; }
        public override ChangeType ChangeType => ChangeType.IndustryPlacement;
    }
}