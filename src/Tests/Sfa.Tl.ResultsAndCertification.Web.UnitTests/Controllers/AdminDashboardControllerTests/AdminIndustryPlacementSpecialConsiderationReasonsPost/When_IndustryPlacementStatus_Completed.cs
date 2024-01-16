using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationReasonsPost
{
    public class When_IndustryPlacementStatus_Completed : When_IndustryPlacementStatus_Invalid
    {
        public When_IndustryPlacementStatus_Completed()
            : base(IndustryPlacementStatus.Completed) { }
    }
}