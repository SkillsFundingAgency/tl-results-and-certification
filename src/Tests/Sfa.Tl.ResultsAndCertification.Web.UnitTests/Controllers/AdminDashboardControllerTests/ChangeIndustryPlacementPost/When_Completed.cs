using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeIndustryPlacementPost
{
    public class When_Completed : When_NotCompletedWithSpecialConsideration
    {
        public When_Completed()
            : base(IndustryPlacementStatus.Completed) { }
    }
}