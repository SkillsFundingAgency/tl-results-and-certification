using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner
{
    public class IndustryPlacement
    {
        public int Id { get; set; }
        public IndustryPlacementStatus Status { get; set; }
    }
}
