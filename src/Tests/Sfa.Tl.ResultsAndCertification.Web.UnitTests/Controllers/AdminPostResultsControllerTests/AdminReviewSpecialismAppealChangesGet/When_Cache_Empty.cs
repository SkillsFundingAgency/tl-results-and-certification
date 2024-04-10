using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminReviewSpecialismAppealChangesGet
{
    public class When_Cache_Empty : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey).Returns(null as AdminOpenPathwayAppealViewModel);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}