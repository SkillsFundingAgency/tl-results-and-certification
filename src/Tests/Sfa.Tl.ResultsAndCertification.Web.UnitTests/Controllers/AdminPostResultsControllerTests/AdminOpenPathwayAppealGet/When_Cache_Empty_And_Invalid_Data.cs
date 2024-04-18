using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminOpenPathwayAppealGet
{
    public class When_Cache_Empty_And_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminOpenPathwayAppealViewModel nullModel = null;

            CacheService.GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey).Returns(nullModel);
            AdminPostResultsLoader.GetAdminOpenPathwayAppealAsync(RegistrationPathwayId, PathwayAssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminOpenPathwayAppealViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminOpenPathwayAppealAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}