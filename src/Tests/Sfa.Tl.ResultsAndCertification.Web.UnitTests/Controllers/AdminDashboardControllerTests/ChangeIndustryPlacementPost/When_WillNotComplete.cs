using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public class When_WillNotComplete : When_NotCompletedWithSpecialConsideration
    {
        public When_WillNotComplete()
            : base(IndustryPlacementStatus.WillNotComplete) { }
    }
}