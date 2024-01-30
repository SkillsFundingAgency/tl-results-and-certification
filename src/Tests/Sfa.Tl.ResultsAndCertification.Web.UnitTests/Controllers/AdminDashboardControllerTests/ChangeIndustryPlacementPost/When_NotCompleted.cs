using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public class When_NotCompleted : When_NotCompletedWithSpecialConsideration
    {
        public When_NotCompleted()
            : base(IndustryPlacementStatus.NotCompleted) { }
    }
}