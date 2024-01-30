using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeIndustryPlacementGet
{
    public class When_BackLink_NotCompleted : When_BackLink_IsCalled
    {
        public When_BackLink_NotCompleted() : base(IndustryPlacementStatus.NotCompleted) { }
    }
}
