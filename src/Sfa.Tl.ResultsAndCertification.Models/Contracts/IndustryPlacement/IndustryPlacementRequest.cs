using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement
{
    public class IndustryPlacementRequest
    {
        public long ProviderUkprn { get; set; }
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public IndustryPlacementStatus IndustryPlacementStatus { get; set; }
        public IndustryPlacementDetails IndustryPlacementDetails { get; set; }

        public string PerformedBy { get; set; }
    }
}
