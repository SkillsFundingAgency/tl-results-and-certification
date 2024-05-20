using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddCoreAppealOutcomeGet
{
    public class When_Cache_Empty_And_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminAddCoreAppealOutcomeViewModel nullModel = null;

            CacheService.GetAsync<AdminAddCoreAppealOutcomeViewModel>(CacheKey).Returns(nullModel);
            AdminPostResultsLoader.GetAdminAddPathwayAppealOutcomeAsync(RegistrationPathwayId, PathwayAssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddCoreAppealOutcomeViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddPathwayAppealOutcomeAsync(RegistrationPathwayId, PathwayAssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}