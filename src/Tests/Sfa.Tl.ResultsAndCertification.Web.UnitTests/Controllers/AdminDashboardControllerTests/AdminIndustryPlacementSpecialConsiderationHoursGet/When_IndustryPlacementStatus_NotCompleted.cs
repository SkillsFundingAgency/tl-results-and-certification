using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationHoursGet
{
    public class When_IndustryPlacementStatus_NotCompleted : When_IndustryPlacementStatus_Invalid
    {
        public When_IndustryPlacementStatus_NotCompleted()
            : base(IndustryPlacementStatus.NotCompleted) { }
    }
}