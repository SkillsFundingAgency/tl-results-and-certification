using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationReasonsPost
{
    public class When_IndustryPlacementStatus_WillNotComplete : When_IndustryPlacementStatus_Invalid
    {
        public When_IndustryPlacementStatus_WillNotComplete()
            : base(IndustryPlacementStatus.WillNotComplete) { }
    }
}