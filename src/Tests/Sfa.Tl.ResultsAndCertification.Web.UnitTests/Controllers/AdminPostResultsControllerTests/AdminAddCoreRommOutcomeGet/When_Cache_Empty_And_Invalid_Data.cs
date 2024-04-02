using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddCoreRommOutcomeAsync
{
    public class When_Cache_Empty_And_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminAddCoreRommOutcomeViewModel nullModel = null;

            CacheService.GetAsync<AdminAddCoreRommOutcomeViewModel>(CacheKey).Returns(nullModel);
            AdminPostResultsLoader.GetAdminAddPathwayRommOutcomeAsync(RegistrationPathwayId, PathwayAssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddCoreRommOutcomeViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddPathwayRommOutcomeAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}