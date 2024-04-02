using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminPostResultsControllerTests.AdminAddRommOutcomeChangeGradeCoreGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            AdminAddRommOutcomeChangeGradeCoreViewModel nullModel = null;

            CacheService.GetAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(CacheKey).Returns(nullModel);
            AdminPostResultsLoader.GetAdminAddRommOutcomeChangeGradeCoreAsync(RegistrationPathwayId, AssessmentId).Returns(nullModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminAddRommOutcomeChangeGradeCoreViewModel>(CacheKey);
            AdminPostResultsLoader.Received(1).GetAdminAddRommOutcomeChangeGradeCoreAsync(RegistrationPathwayId, AssessmentId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}